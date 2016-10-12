using System.IO;
using System.Net;
using System.Net.Sockets;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class TcpQueryFuzzingInput : IFuzzingInput
    {
        string _File;
        /// <summary>
        /// EndPoint
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }
        /// <summary>
        /// Request
        /// </summary>
        public byte[] Request { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Tcp Query"; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        /// <param name="request">Request</param>
        public TcpQueryFuzzingInput(IPEndPoint endPoint, string fileRequest) :
            this(endPoint, string.IsNullOrEmpty(fileRequest) ? null : File.ReadAllBytes(fileRequest))
        {
            _File = fileRequest;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        /// <param name="request">Request</param>
        public TcpQueryFuzzingInput(IPEndPoint endPoint, byte[] request)
        {
            EndPoint = endPoint;
            Request = request;
        }
        /// <summary>
        /// Get Tcp stream
        /// </summary>
        /// <returns></returns>
        public byte[] GetStream()
        {
            TcpClient tcp = new TcpClient();
            tcp.Connect(EndPoint);

            NetworkStream ret = tcp.GetStream();
            if (Request != null)
                ret.Write(Request, 0, Request.Length);

            using (MemoryStream ms = new MemoryStream())
            {
                ret.CopyTo(ms);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_File)) return EndPoint.ToString() + " [" + _File + "]";
            return EndPoint.ToString();
        }
    }
}