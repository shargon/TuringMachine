using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Core.Sockets.Proxy.Enums;
using TuringMachine.Core.Sockets.Proxy.EventArguments;
using TuringMachine.Core.Sockets.Proxy.Interfaces;

namespace TuringMachine.Core.Sockets.Proxy
{
    /// <summary>
    /// Original source "NRepeat" available in github "https://github.com/jeremychild/NRepeat"
    /// </summary>
    public class TcpInvisibleProxy : IProxy, IDisposable
    {
        public IPEndPoint Server { get; set; }
        public IPEndPoint Client { get; set; }
        public int Buffer { get; set; }
        public object Tag { get; set; }
        public bool Running { get { return _AcceptConnectionTask != null && _AcceptConnectionTask.Status == TaskStatus.Running; } }

        TcpListener _Listener;
        Task _AcceptConnectionTask;
        CancellationTokenSource cancellationTokenSource;

        public delegate Stream delOnChangeStream(object sender, Stream stream, ESource owner);

        public event delOnChangeStream OnCreateStream;
        public event EventHandler<ProxyDataEventArgs> OnClientDataSentToServer;
        public event EventHandler<ProxyDataEventArgs> OnServerDataSentToClient;
        public event EventHandler<ProxyByteDataEventArgs> OnClientBytesTransferedToServer;
        public event EventHandler<ProxyByteDataEventArgs> OnServerBytesTransferedToClient;

        List<TcpClient> _Clients = new List<TcpClient>();

        /// <summary>
        /// Start the TCP Proxy
        /// </summary>
        public void Start()
        {
            if (Running) return;

            cancellationTokenSource = new CancellationTokenSource();
            // Check if the listener is null, this should be after the proxy has been stopped
            if (_Listener == null)
            {
                _Listener = new TcpListener(Server.Address, Server.Port);
                _Listener.Start();

                _AcceptConnectionTask = new Task(() => { AcceptConnections(); });
                _AcceptConnectionTask.Start();
            }
        }
        /// <summary>
        /// Accept Connections
        /// </summary>
        void AcceptConnections()
        {
            // If there is an exception we want to output the message to the console for debugging
            try
            {
                // While the Running bool is true, the listener is not null and there is no cancellation requested
                while (_Listener != null && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    TcpClient client = _Listener.AcceptTcpClient();

                    if (client != null)
                    {
                        // Proxy the data from the client to the server until the end of stream filling the buffer.
                        //Task t = new Task(() =>
                        //{
                        ProxyClientConnection(client, Buffer);
                        //});
                        //t.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }

            Stop();
        }
        /// <summary>
        /// Send and receive data between the Client and Server
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serverStream"></param>
        /// <param name="clientStream"></param>
        /// <param name="bufferSize"></param>
        /// <param name="cancellationToken"></param>
        void ProxyClientDataToServer(TcpClient client, Stream serverStream, Stream clientStream, int bufferSize, CancellationToken cancellationToken)
        {
            int clientBytes;
            byte[] message = new byte[bufferSize];

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    clientBytes = clientStream.Read(message, 0, bufferSize);

                    // Client disconnected.
                    if (clientBytes == 0) break;

                    if (OnClientBytesTransferedToServer != null)
                    {
                        byte[] messageTrimed = new byte[clientBytes];
                        Array.Copy(message, 0, messageTrimed, 0, clientBytes);

                        OnClientBytesTransferedToServer(this, new ProxyByteDataEventArgs(messageTrimed, ESource.Client));
                    }

                    serverStream.Write(message, 0, clientBytes);
                    serverStream.Flush();

                    OnClientDataSentToServer?.Invoke(this, new ProxyDataEventArgs(clientBytes));
                }
                catch // (Exception e)
                {
                    // Socket error - exit loop.  Client will have to reconnect.
                    break;
                }
            }

            try { client.Close(); } catch { }
            try { client.Client.Dispose(); } catch { }
            try { serverStream.Dispose(); } catch { }
            try { clientStream.Dispose(); } catch { }

            //Stop();
        }
        /// <summary>
        /// Send and receive data between the Server and Client
        /// </summary>
        /// <param name="serverStream"></param>
        /// <param name="clientStream"></param>
        /// <param name="bufferSize"></param>
        /// <param name="cancellationToken"></param>
        void ProxyServerDataToClient(Stream serverStream, Stream clientStream, int bufferSize, CancellationToken cancellationToken)
        {
            int serverBytes;
            byte[] message = new byte[bufferSize];

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    serverBytes = serverStream.Read(message, 0, bufferSize);

                    // Server disconnected
                    if (serverBytes == 0) break;

                    if (OnServerBytesTransferedToClient != null)
                    {
                        byte[] messageTrimed = new byte[serverBytes];
                        Array.Copy(message, 0, messageTrimed, 0, serverBytes);

                        OnServerBytesTransferedToClient(this, new ProxyByteDataEventArgs(messageTrimed, ESource.Server));
                    }

                    clientStream.Write(message, 0, serverBytes);
                    clientStream.Flush();

                    OnServerDataSentToClient?.Invoke(this, new ProxyDataEventArgs(serverBytes));
                }
                catch // (Exception e)
                {
                    // Server socket error - exit loop.  Client will have to reconnect.
                    break;
                }
            }

            try { serverStream.Dispose(); } catch { }
            try { clientStream.Dispose(); } catch { }

            //Stop();
        }
        /// <summary>
        /// Process the client with a predetermined buffer size
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="bufferSize">Buffer size</param>
        void ProxyClientConnection(TcpClient client, int bufferSize)
        {
            try
            {
                _Clients.Add(client);

                // Handle this client
                // Send the server data to client and client data to server - swap essentially.
                Stream clientStream = client.GetStream();

                TcpClient server = new TcpClient(Client.Address.ToString(), Client.Port);
                Stream serverStream = server.GetStream();

                _Clients.Add(server);

                if (OnCreateStream != null)
                {
                    clientStream = OnCreateStream(this, clientStream, ESource.Client);
                    serverStream = OnCreateStream(this, serverStream, ESource.Server);
                }

                CancellationToken cancellationToken = cancellationTokenSource.Token;

                // Continually do the proxying
                new Task(() => ProxyClientDataToServer(client, serverStream, clientStream, bufferSize, cancellationToken), cancellationToken).Start();
                new Task(() => ProxyServerDataToClient(serverStream, clientStream, bufferSize, cancellationToken), cancellationToken).Start();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Stop the Proxy Server
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                if (_Listener == null || cancellationTokenSource == null) return;

                try { cancellationTokenSource.Cancel(); } catch { }
                cancellationTokenSource = null;

                try { _Listener.Stop(); } catch { }
                try { _Listener.Server.Close(); } catch { }
                try { _Listener.Server.Dispose(); } catch { }

                _Listener = null;
            }

            lock (_Clients)
            {
                while (_Clients.Count > 0)
                {
                    TcpClient tcp = _Clients.FirstOrDefault();
                    if (tcp == null) return;

                    try { tcp.Close(); } catch { }
                    try { tcp.Client.Dispose(); } catch { }

                    _Clients.Remove(tcp);
                }
            }

            // Wait stop
            if (_AcceptConnectionTask != null)
            {
                try { _AcceptConnectionTask.Wait(); }
                catch { }
                try { _AcceptConnectionTask.Dispose(); }
                catch { }

                _AcceptConnectionTask = null;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server EndPoint</param>
        /// <param name="client">Client EndPoint</param>
        /// <param name="buffer">Buffer</param>
        public TcpInvisibleProxy(IPEndPoint server, IPEndPoint client, int buffer)
        {
            Server = server;
            Client = client;
            Buffer = buffer;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server EndPoint</param>
        /// <param name="client">Client EndPoint</param>
        public TcpInvisibleProxy(IPEndPoint server, IPEndPoint client) : this(server, client, 4096) { }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { Stop(); }
    }
}