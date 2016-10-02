using System;
using System.Net;
using System.Net.Sockets;

namespace TuringMachine.Client
{
    public class TcpForwarder : IDisposable
    {
        const int BufferLength = 8192;

        Socket _MainSocket = null;

        /// <summary>
        /// Local EndPoint
        /// </summary>
        public IPEndPoint LocalEndPoint { get; private set; }
        /// <summary>
        /// Remote EndPoint
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            if (_MainSocket == null) return;

            try { _MainSocket.Close(); } catch { }
            try { _MainSocket.Dispose(); } catch { }
            _MainSocket = null;
        }
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="local">Local EndPoint</param>
        /// <param name="remote">Remote EndPoint</param>
        public void Start(IPEndPoint local, IPEndPoint remote)
        {
            Stop();

            _MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _MainSocket.Bind(local);
            _MainSocket.Listen(50);

            LocalEndPoint = local;
            RemoteEndPoint = remote;

            _MainSocket.BeginAccept(OnAccept, _MainSocket);
        }
        void OnAccept(IAsyncResult result)
        {
            // Get Sockets
            Socket main, source;
            try
            {
                main = (Socket)result.AsyncState;
                if (main == null) return;
                source = main.EndAccept(result);
                if (source == null) return;
            }
            catch
            {
                return;
            }

            // Connect
            try
            {
                TcpForwarder destination = new TcpForwarder();
                destination._MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                TcpForwarderState state = new TcpForwarderState(true, source, destination._MainSocket, BufferLength);
                destination.Connect(RemoteEndPoint, source);
                source.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);

            }
            catch { }
            // Re-accept
            try { main.BeginAccept(OnAccept, main); } catch { }
        }
        void Connect(EndPoint remoteEndpoint, Socket destination)
        {
            TcpForwarderState state = new TcpForwarderState(false, _MainSocket, destination, BufferLength);
            _MainSocket.Connect(remoteEndpoint);
            _MainSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, OnDataReceive, state);
        }
        static void OnDataReceive(IAsyncResult result)
        {
            TcpForwarderState state = (TcpForwarderState)result.AsyncState;
            try
            {
                int bytesRead = state.SourceSocket.EndReceive(result);
                if (bytesRead > 0)
                {
                    state.DestinationSocket.Send(state.Buffer, bytesRead, SocketFlags.None);
                    state.SourceSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);
                }
            }
            catch
            {
                state.DestinationSocket.Close();
                state.SourceSocket.Close();
            }
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}