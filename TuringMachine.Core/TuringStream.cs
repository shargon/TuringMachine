using System;
using System.IO;
using TuringMachine.Core.Sockets;
using TuringMachine.Core.Sockets.Messages.Requests;
using TuringMachine.Core.Sockets.Messages.Responses;

namespace TuringMachine.Core
{
    /// <summary>
    /// Remote Turing stream
    /// </summary>
    public class TuringStream : Stream
    {
        TuringSocket _Socket;
        Guid _StreamId;

        bool _CanRead, _CanSeek, _CanWrite, _CanTimeout;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket</param>
        public TuringStream(TuringSocket socket)
        {
            _Socket = socket;

            socket.EnqueueMessages = true;
            socket.SendMessage(new OpenStreamMessageRequest());

            OpenStreamMessageResponse ret = socket.ReadMessage<OpenStreamMessageResponse>();

            _StreamId = ret.Id;
            _CanRead = ret.CanRead;
            _CanSeek = ret.CanSeek;
            _CanTimeout = ret.CanTimeout;
            _CanWrite = ret.CanWrite;
        }
        public override bool CanRead { get { return _CanRead; } }
        public override bool CanSeek { get { return _CanSeek; } }
        public override bool CanWrite { get { return _CanWrite; } }
        public override bool CanTimeout { get { return _CanTimeout; } }

        public override long Length
        {
            get
            {
                _Socket.SendMessage(new GetStreamLengthMessageRequest(_StreamId));

                LongMessageResponse ret = _Socket.ReadMessage<LongMessageResponse>();
                return ret.Result;
            }
        }
        public override long Position
        {
            get
            {
                _Socket.SendMessage(new GetStreamPositionMessageRequest(_StreamId));

                LongMessageResponse ret = _Socket.ReadMessage<LongMessageResponse>();
                return ret.Result;
            }
            set
            {
                _Socket.SendMessage(new SetStreamMessageRequest(_StreamId) { Value = value, ValueType = SetStreamMessageRequest.EMode.Position });

                BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
                if (!ret.Result) throw (new Exception("Error setting stream position"));
            }
        }
        public override void Close()
        {
            _Socket.SendMessage(new CloseStreamMessageRequest(_StreamId));

            BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
            if (!ret.Result) throw (new Exception("Error clossing stream"));
        }
        public override void SetLength(long value)
        {
            _Socket.SendMessage(new SetStreamMessageRequest(_StreamId) { Value = value, ValueType = SetStreamMessageRequest.EMode.Length });

            BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
            if (!ret.Result) throw (new Exception("Error setting stream length"));
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    {
                        Position = offset;
                        return Position;
                    }
                case SeekOrigin.Current:
                    {
                        long p = Position;
                        p += offset;
                        Position = p;
                        return p;
                    }
                case SeekOrigin.End:
                    {
                        long l = Length;
                        l = Math.Max(0, l - offset);
                        Position = l;
                        return l;
                    }
            }
            return -1;
        }
        public override void Flush()
        {
            _Socket.SendMessage(new FlushStreamMessageRequest(_StreamId));

            BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
            if (!ret.Result) throw (new Exception("Error flushing stream"));
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count <= 0)
                return 0;

            _Socket.SendMessage(new StreamReadMessageRequest(_StreamId) { Length = count });

            ByteArrayMessageResponse ret = _Socket.ReadMessage<ByteArrayMessageResponse>();
            if (ret.Result == null)
                throw (new Exception("Error reading stream"));

            if (ret.Result.Length == 0) return 0;

            Array.Copy(ret.Result, 0, buffer, offset, ret.Result.Length);
            return ret.Result.Length;
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count <= 0) return;

            byte[] send;
            if (offset != 0 || count != buffer.Length)
            {
                // Copy buffer
                send = new byte[count];
                Array.Copy(buffer, offset, send, 0, count);
            }
            else
            {
                send = buffer;
            }
            _Socket.SendMessage(new StreamWriteMessageRequest(_StreamId) { Data = send });

            BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
            if (!ret.Result) throw (new Exception("Error writting stream"));
        }
    }
}