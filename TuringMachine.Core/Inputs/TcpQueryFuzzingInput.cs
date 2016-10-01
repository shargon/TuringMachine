using System.IO;
using System.Net;
using System.Net.Sockets;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class TcpQueryFuzzingInput : IFuzzingInput
    {
        /// <summary>
        /// EndPoint
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Tcp Query"; } }
        /// <summary>
        /// IsSelectable
        /// </summary>
        public bool IsSelectable { get { return true; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        public TcpQueryFuzzingInput(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }
        /// <summary>
        /// Get Tcp stream
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            TcpClient tcp = new TcpClient(EndPoint);
            return tcp.GetStream();
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return EndPoint.ToString();
        }
    }
}