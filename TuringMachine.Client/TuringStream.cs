using System;
using System.IO;
using TuringMachine.Client.Sockets;
using TuringMachine.Client.Sockets.Messages;

namespace TuringMachine.Client
{
    /// <summary>
    /// Remote Turing stream
    /// </summary>
    public class TuringStream : Stream
    {
        TuringSocket _Socket;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket</param>
        public TuringStream(TuringSocket socket)
        {
            _Socket = socket;

            socket.EnqueueMessages = true;
            socket.SendMessage(new TuringConfigMessage() { InputType = TuringConfigMessage.EInputType.Random });

            TuringMessage ret = socket.ReadMessage();
        }
        public override bool CanRead { get { throw new NotImplementedException(); } }
        public override bool CanSeek { get { throw new NotImplementedException(); } }
        public override bool CanWrite { get { throw new NotImplementedException(); } }
        public override long Length { get { throw new NotImplementedException(); } }
        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public override void Flush() { throw new NotImplementedException(); }
        public override int Read(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }
        public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }
        public override void SetLength(long value) { throw new NotImplementedException(); }
        public override void Write(byte[] buffer, int offset, int count) { throw new NotImplementedException(); }
    }
}