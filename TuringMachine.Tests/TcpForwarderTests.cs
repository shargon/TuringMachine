using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading;
using TuringMachine.Client.Sockets.Forwarder;

namespace TuringMachine.Tests
{
    [TestClass]
    public class TcpForwarderTests
    {
        [TestMethod]
        public void TestTcpForwarderTest()
        {
            TcpForwarder c = new TcpForwarder() { };
            c.Start(new IPEndPoint(IPAddress.Any, 8088), new IPEndPoint(IPAddress.Parse("94.23.100.178"), 80));
            
            Thread.Sleep(30000);
            c.Stop();
            c.Dispose();
        }
    }
}