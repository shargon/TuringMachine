using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Core.Sockets.Proxy.Enums;

namespace NRepeat
{
    public class TcpProxy : IProxy, IDisposable
    {
        public IPEndPoint Server { get; set; }
        public IPEndPoint Client { get; set; }
        public int Buffer { get; set; }
        public object Tag { get; set; }
        public bool Running { get; set; }

        private static TcpListener listener;

        private CancellationTokenSource cancellationTokenSource;
        public delegate Stream delOnChangeStream(object sender, Stream stream, ESource owner);

        public event delOnChangeStream OnCreateStream;
        public event EventHandler<ProxyDataEventArgs> OnClientDataSentToServer;
        public event EventHandler<ProxyDataEventArgs> OnServerDataSentToClient;
        public event EventHandler<ProxyByteDataEventArgs> OnClientBytesTransferedToServer;
        public event EventHandler<ProxyByteDataEventArgs> OnServerBytesTransferedToClient;

        /// <summary>
        /// Start the TCP Proxy
        /// </summary>
        public async void Start()
        {
            if (Running == false)
            {
                cancellationTokenSource = new CancellationTokenSource();
                // Check if the listener is null, this should be after the proxy has been stopped
                if (listener == null)
                {
                    await AcceptConnections();
                }
            }
        }
        /// <summary>
        /// Accept Connections
        /// </summary>
        /// <returns></returns>
        private async Task AcceptConnections()
        {
            listener = new TcpListener(Server.Address, Server.Port);
            var bufferSize = Buffer; // Get the current buffer size on start
            listener.Start();
            Running = true;

            // If there is an exception we want to output the message to the console for debugging
            try
            {
                // While the Running bool is true, the listener is not null and there is no cancellation requested
                while (Running && listener != null && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var client = await listener.AcceptTcpClientAsync().WithWaitCancellation(cancellationTokenSource.Token);
                    if (client != null)
                    {
                        // Proxy the data from the client to the server until the end of stream filling the buffer.
                        await ProxyClientConnection(client, bufferSize);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            listener.Stop();
        }

        /// <summary>
        /// Send and receive data between the Client and Server
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serverStream"></param>
        /// <param name="clientStream"></param>
        /// <param name="bufferSize"></param>
        /// <param name="cancellationToken"></param>
        private void ProxyClientDataToServer(TcpClient client, Stream serverStream, Stream clientStream, int bufferSize, CancellationToken cancellationToken)
        {
            byte[] message = new byte[bufferSize];
            int clientBytes;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    clientBytes = clientStream.Read(message, 0, bufferSize);
                    if (OnClientBytesTransferedToServer != null)
                    {
                        var messageTrimed = message.Reverse().SkipWhile(x => x == 0).Reverse().ToArray();
                        OnClientBytesTransferedToServer(this, new ProxyByteDataEventArgs(messageTrimed, ESource.Client));
                    }
                }
                catch
                {
                    // Socket error - exit loop.  Client will have to reconnect.
                    break;
                }
                if (clientBytes == 0)
                {
                    // Client disconnected.
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
        private void ProxyServerDataToClient(Stream serverStream, Stream clientStream, int bufferSize, CancellationToken cancellationToken)
        {
            byte[] message = new byte[bufferSize];
            int serverBytes;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    serverBytes = serverStream.Read(message, 0, bufferSize);
                    if (OnServerBytesTransferedToClient != null)
                    {
                        var messageTrimed = message.Reverse().SkipWhile(x => x == 0).Reverse().ToArray();
                        OnServerBytesTransferedToClient(this, new ProxyByteDataEventArgs(messageTrimed, ESource.Server));
                    }
                    clientStream.Write(message, 0, serverBytes);
                }
                catch
                {
                    // Server socket error - exit loop.  Client will have to reconnect.
                    break;
                }
                if (serverBytes == 0)
                {
                    // server disconnected.
                    break;
                }
                OnServerDataSentToClient?.Invoke(this, new ProxyDataEventArgs(serverBytes));
            }
            message = null;
        }
        /// <summary>
        /// Process the client with a predetermined buffer size
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        private async Task ProxyClientConnection(TcpClient client, int bufferSize)
        {
            // Handle this client
            // Send the server data to client and client data to server - swap essentially.
            Stream clientStream = client.GetStream();

            if (OnCreateStream != null)
                clientStream = OnCreateStream(this, clientStream, ESource.Client);

            TcpClient server = new TcpClient(Client.Address.ToString(), Client.Port);
            Stream serverStream = server.GetStream();

            if (OnCreateStream != null)
                serverStream = OnCreateStream(this, serverStream, ESource.Client);

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            try
            {
                // Continually do the proxying
                new Task(() => ProxyClientDataToServer(client, serverStream, clientStream, bufferSize, cancellationToken), cancellationToken).Start();
                new Task(() => ProxyServerDataToClient(serverStream, clientStream, bufferSize, cancellationToken), cancellationToken).Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                cancellationTokenSource = null;

            }
        }

        public void Dispose() { Stop(); }

        public TcpProxy(IPEndPoint server, IPEndPoint client, int buffer)
        {
            Server = server;
            Client = client;
            Buffer = buffer;
        }
        public TcpProxy(IPEndPoint server, IPEndPoint client) : this(server, client, 4096) { }
    }
}