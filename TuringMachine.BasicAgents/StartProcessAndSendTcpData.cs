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

        public override ICrashDetector CreateDetector(TuringSocket socket)
        {
            // Create process
            return new WERDetector(new ProcessStartInfo(FileName, Arguments)
            {
                //CreateNoWindow = true,
                //WindowStyle = ProcessWindowStyle.Hidden
            });
        }
        public override void OnRun(TuringSocket socket)
        {
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
            }
        }
        /// <summary>
        /// Check if can reconnect (¿its alive?)
        /// </summary>
        /// <param name="socket">Socket</param>
        public override bool GetItsAlive(TuringSocket socket)
        {
            try
            {
                using (TcpClient ret = new TcpClient())
                    ret.Connect(ConnectTo);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}