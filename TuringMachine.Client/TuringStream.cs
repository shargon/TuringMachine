using System;
using System.IO;
using System.Net;
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
        TuringStream(TuringSocket socket)
        {
            _Socket = socket;
        }
        /// <summary>
        /// Get regular Input
        /// </summary>
        public static TuringStream Create(IPEndPoint remote)
        {
            TuringSocket t = TuringSocket.ConnectTo(remote);
            t.MessagesToList = true;

            TuringStream ret = new TuringStream(t);
            t.SendMessage(new TuringConfigMessage() { InputType = TuringConfigMessage.EInputType.Random });

            return ret;
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