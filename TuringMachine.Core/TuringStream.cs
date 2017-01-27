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
        Stream _Base;
        TuringSocket _Socket;
        Guid _StreamId;

        bool _CanRead, _CanSeek, _CanWrite, _CanTimeout;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket</param>
        public TuringStream(TuringSocket socket) : this(socket, null) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="stream">Stream</param>
        public TuringStream(TuringSocket socket, Stream stream)
        {
            _Base = stream;
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
                if (_Base != null) return _Base.Length;

                _Socket.SendMessage(new GetStreamLengthMessageRequest(_StreamId));

                LongMessageResponse ret = _Socket.ReadMessage<LongMessageResponse>();
                return ret.Result;
            }
        }
        public override long Position
        {
            get
            {
                if (_Base != null) return _Base.Position;

                _Socket.SendMessage(new GetStreamPositionMessageRequest(_StreamId));

                LongMessageResponse ret = _Socket.ReadMessage<LongMessageResponse>();
                return ret.Result;
            }
            set
            {
                if (_Base != null)
                {
                    _Base.Position = value;
                }
                else
                {
                    _Socket.SendMessage(new SetStreamMessageRequest(_StreamId) { Value = value, ValueType = SetStreamMessageRequest.EMode.Position });

                    BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
                    if (!ret.Result) throw (new Exception("Error setting stream position"));
                }
            }
        }
        public override void Close()
        {
            if (_Base != null)
            {
                _Base.Close();
                _Base.Dispose();
                _Base = null;
            }

            _Socket.SendMessage(new CloseStreamMessageRequest(_StreamId));

            BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
            if (!ret.Result) throw (new Exception("Error clossing stream"));
        }
        public override void SetLength(long value)
        {
            if (_Base != null)
            {
                _Base.SetLength(value);
            }
            else
            {
                _Socket.SendMessage(new SetStreamMessageRequest(_StreamId) { Value = value, ValueType = SetStreamMessageRequest.EMode.Length });

                BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
                if (!ret.Result) throw (new Exception("Error setting stream length"));
            }
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
            if (_Base != null) _Base.Flush();
            else
            {
                _Socket.SendMessage(new FlushStreamMessageRequest(_StreamId));

                BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
                if (!ret.Result) throw (new Exception("Error flushing stream"));
            }
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count <= 0)
                return 0;

            byte[] append = null;
            if (_Base != null)
            {
                append = new byte[count];
                count = _Base.Read(append, 0, count);

                if (count != append.Length)
                    Array.Resize(ref append, count);
            }

            _Socket.SendMessage(new StreamReadMessageRequest(_StreamId) { Length = count, PreAppend = append, PreAppendReSeek = true });

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

            //byte[] send;
            //if (offset != 0 || count != buffer.Length)
            //{
            //    // Copy buffer
            //    send = new byte[count];
            //    Array.Copy(buffer, offset, send, 0, count);
            //}
            //else
            //{
            //    send = buffer;
            //}
            //_Socket.SendMessage(new StreamWriteMessageRequest(_StreamId) { Data = send });

            //BoolMessageResponse ret = _Socket.ReadMessage<BoolMessageResponse>();
            //if (!ret.Result) throw (new Exception("Error writting stream"));

            if (_Base != null)
            {
                _Base.Write(buffer, offset, count);
            }
        }
    }
}