using System;
using System.IO;
using System.Net;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class TcpProxyFuzzingInput : IFuzzingInput
    {
        /// <summary>
        /// EndPoint
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Tcp Proxy"; } }
        /// <summary>
        /// IsSelectable
        /// </summary>
        public bool IsSelectable { get { return false; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        public TcpProxyFuzzingInput(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }
        /// <summary>
        /// Get Tcp stream
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            throw (new NotImplementedException());
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