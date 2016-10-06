using System.IO;
using System.Net;
using System.Net.Sockets;
using TuringMachine.Client;
using TuringMachine.Client.Detectors;
using TuringMachine.Client.Detectors.Windows;
using TuringMachine.Client.Sockets;

namespace TuringMachine.BasicAgents
{
    public class StartProcessAndSendTcpData : ITuringMachineAgent
    {
        /// <summary>
        /// Process path
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Arguments
        /// </summary>
        public string Arguments { get; set; }
        /// <summary>
        /// Connect to
        /// </summary>
        public IPEndPoint ConnectTo { get; set; }

        public override ICrashDetector Run(TuringSocket socket)
        {
            // Create process
            WERDetector process = new WERDetector(FileName, Arguments);

            // Create client
            using (TcpClient ret = new TcpClient())
            {
                ret.Connect(ConnectTo);

                // Fuzzer stream
                using (TuringStream stream = new TuringStream(socket))
                    try
                    {
                        // Try send all we can
                        using (Stream sr = ret.GetStream()) stream.CopyTo(sr);
                    }
                    catch { }

                // WER Checker
                return process;
            }
        }
    }
}