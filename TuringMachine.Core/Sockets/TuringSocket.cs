using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TuringMachine.Core.Collections;
using TuringMachine.Core.Sockets.Enums;
using TuringMachine.Core.Sockets.Messages;

namespace TuringMachine.Core.Sockets
{
    public class TuringSocket : IDisposable
    {
        const int BufferLength = 8192;

        public delegate void delOnMessage(TuringSocket sender, TuringMessage message);
        public event delOnMessage OnMessage;

        ConcurrentQueue<TuringMessage> _Readed = new ConcurrentQueue<TuringMessage>();
        AutoResetEvent _Signal = new AutoResetEvent(false);

        Socket _Socket;
        /// <summary>
        /// True for Enqueue messages
        /// </summary>
        public bool EnqueueMessages { get; set; }
        /// <summary>
        /// Variables
        /// </summary>
        public VariableCollection<string, object> Variables { get; private set; }
        /// <summary>
        /// ListenEndPoint
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }
        /// <summary>
        /// Index to variables
        /// </summary>
        /// <param name="name">Variable name</param>
        public object this[string name]
        {
            get { return Variables[name]; }
            set { Variables[name] = value; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="endPoint">EndPoint</param>
        protected TuringSocket(Socket socket, IPEndPoint endPoint)
        {
            _Socket = socket;
            EndPoint = endPoint;
            Variables = new VariableCollection<string, object>();
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
            if (message == null || _Socket == null) return;

            lock (_Socket)
            {
                byte[] data = message.GetData();

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
            state.Source._Socket.BeginReceive(state.Buffer, state.Index, state.Buffer.Length - state.Index, 0, state.Source.OnDataReceive, state);
        }
        /// <summary>
        /// Read message
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public T ReadMessage<T>() where T : TuringMessage
        {
            if (_Signal == null) return null;

            TuringMessage ret = null;

            _Signal.WaitOne();
            lock (_Readed)
            {
                while (_Readed != null && !_Readed.TryDequeue(out ret)) { Thread.Sleep(1); }
            }

            if (ret == null) throw (new Exception("Disconnected"));
            if (ret is T) return (T)ret;
            if (ret is ExceptionMessage)
                throw (new Exception(((ExceptionMessage)ret).Error));

            throw (new Exception("Bad response"));
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
            catch (Exception e)
            {

            }

            // Re-accept
            try { main._Socket.BeginAccept(OnAccept, main); } catch { }
        }
        void RaiseOnMessage(TuringSocket sender, TuringMessage message)
        {
            if (message == null) return;

            // Enqueue
            if (EnqueueMessages)
            {
                lock (_Readed) { _Readed.Enqueue(message); }
                _Signal.Set();
            }
            OnMessage?.Invoke(sender, message);
        }
        void OnDataReceive(IAsyncResult result)
        {
            TuringMessageState state = (TuringMessageState)result.AsyncState;
            try
            {
                int bytesRead = state.Source._Socket.EndReceive(result);
                if (bytesRead > 0)
                {
                    TuringMessage msg = state.CheckData(bytesRead);
                    if (msg != null) RaiseOnMessage(state.Source, msg);
                    ReadMessageAsync(state);
                }
            }
            catch (Exception e)
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

            if (_Readed != null)
            {
                try
                {
                    TuringMessage i;
                    while (_Readed.Count > 0)
                        _Readed.TryDequeue(out i);
                }
                catch { }
                _Readed = null;
            }
            if (_Socket != null)
            {
                try { _Socket.Close(); } catch { }
                try { _Socket.Dispose(); } catch { }
                _Socket = null;
            }
            if (_Signal != null)
            {
                try { _Signal.Dispose(); } catch { }
                _Signal = null;
            }
            if (Variables != null)
            {
                foreach (object o in Variables.Values)
                    try
                    {
                        if (o != null && o is IDisposable)
                            ((IDisposable)o).Dispose();
                    }
                    catch { }
            }
        }
    }
}