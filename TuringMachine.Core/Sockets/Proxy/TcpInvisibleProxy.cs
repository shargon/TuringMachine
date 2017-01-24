using System;
using System.IO;
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
        public bool Running { get; set; }

        static TcpListener listener;
        CancellationTokenSource cancellationTokenSource;

        public delegate Stream delOnChangeStream(object sender, Stream stream, ESource owner);

        public event delOnChangeStream OnCreateStream;
        public event EventHandler<ProxyDataEventArgs> OnClientDataSentToServer;
        public event EventHandler<ProxyDataEventArgs> OnServerDataSentToClient;
        public event EventHandler<ProxyByteDataEventArgs> OnClientBytesTransferedToServer;
        public event EventHandler<ProxyByteDataEventArgs> OnServerBytesTransferedToClient;

        /// <summary>
        /// Start the TCP Proxy
        /// </summary>
        public void Start()
        {
            if (Running == false)
            {
                cancellationTokenSource = new CancellationTokenSource();
                // Check if the listener is null, this should be after the proxy has been stopped
                if (listener == null)
                {
                    Running = true;
                    Task t = new Task(() => { AcceptConnections(); });
                    t.Start();
                }
            }
        }
        /// <summary>
        /// Accept Connections
        /// </summary>
        void AcceptConnections()
        {
            listener = new TcpListener(Server.Address, Server.Port);
            listener.Start();

            // If there is an exception we want to output the message to the console for debugging
            try
            {
                // While the Running bool is true, the listener is not null and there is no cancellation requested
                while (Running && listener != null && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    TcpClient client = listener.AcceptTcpClient();

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
                }
                catch
                {
                    // Socket error - exit loop.  Client will have to reconnect.
                    break;
                }

                serverStream.Write(message, 0, clientBytes);

                OnClientDataSentToServer?.Invoke(this, new ProxyDataEventArgs(clientBytes));
            }

            message = null;
            client.Close();
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
                }
                catch
                {
                    // Server socket error - exit loop.  Client will have to reconnect.
                    break;
                }

                OnServerDataSentToClient?.Invoke(this, new ProxyDataEventArgs(serverBytes));
            }
            message = null;
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
                // Handle this client
                // Send the server data to client and client data to server - swap essentially.
                Stream clientStream = client.GetStream();

                if (OnCreateStream != null)
                    clientStream = OnCreateStream(this, clientStream, ESource.Client);

                TcpClient server = new TcpClient(Client.Address.ToString(), Client.Port);
                Stream serverStream = server.GetStream();

                if (OnCreateStream != null)
                    serverStream = OnCreateStream(this, serverStream, ESource.Server);

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
            if (listener != null && cancellationTokenSource != null)
            {
                try
                {
                    Running = false;
                    listener.Stop();
                    cancellationTokenSource.Cancel();
                    listener = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                cancellationTokenSource = null;
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