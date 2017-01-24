using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Threading;
using TuringMachine.Core.Sockets.Proxy;
using TuringMachine.Core.Sockets.Proxy.Enums;

namespace TuringMachine.Tests
{
    [TestClass]
    public class TcpProxyTests
    {
        [TestMethod]
        public void TestTcpForwarderTest()
        {
            using (TcpInvisibleProxy tcp = new TcpInvisibleProxy(new IPEndPoint(IPAddress.Any, 7000), new IPEndPoint(IPAddress.Parse("192.168.1.5"), 3306)))
            {
                tcp.OnCreateStream += Tcp_OnCreateStream;
                tcp.Start();
                Thread.Sleep(30000);
            }
        }
        Stream Tcp_OnCreateStream(object sender, Stream stream, ESource owner)
        {
            return stream;
        }
    }
}