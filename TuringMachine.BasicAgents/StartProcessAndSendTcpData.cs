using System.Diagnostics;
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
        
        public override ICrashDetector Run(TuringSocket socket, int taskNumber)
        {
            // Create process
            using (Process start = Process.Start(FileName, Arguments))
            {
                // Fuzzer stream
                using (TuringStream stream = new TuringStream(socket))
                {
                    // Create client
                    using (TcpClient ret = new TcpClient())
                    {
                        ret.Connect(ConnectTo);

                        try
                        {
                            // Try send all we can
                            using (Stream sr = ret.GetStream()) stream.CopyTo(sr);
                        }
                        catch { }
                    }

                    // Wer Check
                    return new WERDetector(start.Id);
                }
            }
        }
    }
}