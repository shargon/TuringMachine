using System.IO;
using System.Net;
using System.Net.Sockets;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class TcpFuzzingInput : IFuzzingInput
    {
        /// <summary>
        /// EndPoint
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Tcp"; } }
       
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        public TcpFuzzingInput(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        public Stream GetStream()
        {
            TcpClient tcp = new TcpClient(EndPoint);
            return tcp.GetStream();
        }

        public override string ToString()
        {
            return EndPoint.ToString();
        }
    }
}