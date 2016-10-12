using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Text;
using TuringMachine.Core;
using TuringMachine.Core.Inputs;

namespace TuringMachine.Tests
{
    [TestClass]
    public class InputTests
    {
        [TestMethod]
        public void TestExecutionFuzzingInput()
        {
            ExecutionFuzzingInput c = new ExecutionFuzzingInput("cmd.exe", "/C dir c:") { };

            MemoryStream ms = new MemoryStream(c.GetStream());

            string ret = Encoding.ASCII.GetString(ms.ToArray());
        }
        [TestMethod]
        public void FileFuzzingInput()
        {
            FileFuzzingInput c = new FileFuzzingInput("d:\\bof\\vulnserver.fmut") { };

            MemoryStream ms = new MemoryStream(c.GetStream());

            string ret = Encoding.ASCII.GetString(ms.ToArray());
        }
        [TestMethod]
        public void TestTcpQueryFuzzingInput()
        {
            TcpQueryFuzzingInput c = new TcpQueryFuzzingInput(new IPEndPoint(IPAddress.Parse("216.58.210.3"), 80),
                Encoding.ASCII.GetBytes(
@"GET / HTTP/1.1
Host: www.google.es
Connection: close
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
Accept-Encoding: gzip, deflate, sdch
Accept-Language: en-US,en;q=0.8,es;q=0.6

"));

            MemoryStream ms = new MemoryStream(c.GetStream());

            string ret = Encoding.ASCII.GetString(ms.ToArray());
        }
    }
}