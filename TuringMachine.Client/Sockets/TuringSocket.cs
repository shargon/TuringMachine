using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets
{
    public class TuringSocket : IDisposable
    {
        List<TuringMessage> _Messages;

        const int BufferLength = 8192;

        public delegate void delOnMessage(TuringSocket sender, TuringMessage message);
        public event delOnMessage OnMessage;

        Socket _Socket;

        /// <summary>
        /// ListenEndPoint
        /// </summary>
        IPEndPoint EndPoint { get; set; }
        /// <summary>
        /// Append Messages to List
        /// </summary>
        public bool MessagesToList { get; set; }
        /// <summary>
        /// Messages
        /// </summary>
        public List<TuringMessage> Messages { get { return _Messages; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="endPoint">EndPoint</param>
        protected TuringSocket(Socket socket, IPEndPoint endPoint)
        {
            _Socket = socket;
            EndPoint = endPoint;
            _Messages = new List<TuringMessage>();
        }
        /// <summary>
        /// Bind socket
        /// </summary>
        /// <param name="local">Local EndPoint</param>
        public static TuringSocket Bind(IPEndPoint local)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(local);
            socket.Listen(50);

            TuringSocket ret = new TuringSocket(socket, local);

            socket.BeginAccept(ret.OnAccept, ret);
            return ret;
        }
        /// <summary>
        /// Connect to
        /// </summary>
        /// <param name="remote">Remote EndPoint</param>
        public static TuringSocket ConnectTo(IPEndPoint remote)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(remote);

            TuringSocket ret = new TuringSocket(socket, remote);

            // WaitMessage
            ReadMessageAsync(new TuringMessageState(ret));

            return ret;
        }
        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="message">Message</param>
        public void SendMessage(TuringMessage message)
        {
            if (message == null) return;

            byte[] data = message.GetData();

            lock (_Socket)
            {
                // Send header
                _Socket.Send(message.GetHeader(data == null ? 0 : data.Length), 0, TuringMessage.HeaderLength, SocketFlags.None);
                // Send data
                if (data != null) _Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }
        /// <summary>
        /// Read Message sync
        /// </summary>
        static void ReadMessageAsync(TuringMessageState state)
        {
            try
            {
                state.Source._Socket.BeginReceive(state.Buffer, state.Index, state.Buffer.Length, 0, state.Source.OnDataReceive, state);
            }
            catch { }
        }
        /// <summary>
        /// Read Message sync
        /// </summary>
        public TuringMessage ReadMessage()
        {
            return null;
        }
        void OnAccept(IAsyncResult result)
        {
            // Get Sockets
            TuringSocket main;
            Socket source;
            try
            {
                main = (TuringSocket)result.AsyncState;
                if (main == null) return;

                source = main._Socket.EndAccept(result);
                if (source == null) return;
            }
            catch { return; }

            // Connect
            try
            {
                TuringSocket ret = new TuringSocket(source, (IPEndPoint)source.RemoteEndPoint);
                // CopyEvents
                ret.OnMessage += RaiseOnMessage;

                ReadMessageAsync(new TuringMessageState(ret));
            }
            catch { }

            // Re-accept
            try { main._Socket.BeginAccept(OnAccept, main); } catch { }
        }
        void RaiseOnMessage(TuringSocket sender, TuringMessage message)
        {
            if (MessagesToList) _Messages.Add(message);
            OnMessage?.Invoke(sender, message);
        }
        void OnDataReceive(IAsyncResult result)
        {
            TuringMessageState state = (TuringMessageState)result.AsyncState;
            try
            {
                int bytesRead = state.Source._Socket.EndReceive(result);
                if (bytesRead > 0)
                    state.CheckData(bytesRead);

                switch (state.State)
                {
                    case ETuringMessageState.Full:
                        {
                            RaiseOnMessage(state.Source, TuringMessage.Create(state.MessageType, state.Buffer));
                            break;
                        }
                }

                ReadMessageAsync(state);
            }
            catch
            {
                state.Source.Dispose();
            }
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            if (_Socket == null) return;

            try { _Socket.Close(); } catch { }
            try { _Socket.Dispose(); } catch { }

            _Socket = null;
        }
    }
}