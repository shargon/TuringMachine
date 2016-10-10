using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading;
using TuringMachine.Core.Sockets;
using TuringMachine.Core.Sockets.Messages.Requests;

namespace TuringMachine.Tests
{
    [TestClass]
    public class TuringSocketTests
    {
        bool isOk = false;
        [TestMethod]
        public void TestTuringSocket()
        {
            using (TuringSocket server = TuringSocket.Bind(new IPEndPoint(IPAddress.Any, 9787)))
            {
                server.OnMessage += Server_OnMessage;

                using (TuringSocket client = TuringSocket.ConnectTo(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9787)))
                {
                    client.OnMessage += Client_OnMessage;
                    client.SendMessage(new OpenStreamMessageRequest() { });

                    for (int x = 0; x < 20 && !isOk; x++)
                        Thread.Sleep(1000);
                }
            }

            if (!isOk) throw (new System.Exception());
        }
        void Server_OnMessage(TuringSocket sender, TuringMessage message)
        {
            sender.SendMessage(new CloseStreamMessageRequest() { });
        }
        void Client_OnMessage(TuringSocket sender, TuringMessage message)
        {
            isOk = message is CloseStreamMessageRequest;
        }
    }
}