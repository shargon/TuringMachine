using System.Net.Sockets;

namespace TuringMachine.Core.Sockets.Forwarder
{
    class TcpForwarderState
    {
        /// <summary>
        /// Is Listener
        /// </summary>
        public bool IsListener { get; private set; }
        /// <summary>
        /// Source Socket
        /// </summary>
        public Socket SourceSocket { get; private set; }
        /// <summary>
        /// Destination socket
        /// </summary>
        public Socket DestinationSocket { get; private set; }
        /// <summary>
        /// Buffer
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isListener">Is listener</param>
        /// <param name="source">Source socket</param>
        /// <param name="destination">Destination socket</param>
        /// <param name="bufferLength">Buffer length</param>
        public TcpForwarderState(bool isListener, Socket source, Socket destination, int bufferLength)
        {
            IsListener = isListener;
            SourceSocket = source;
            DestinationSocket = destination;
            Buffer = new byte[bufferLength];
        }
    }
}