using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TuringMachine.Core;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Detectors;
using TuringMachine.Core.Detectors.Windows;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets;

namespace TuringMachine.BasicAgents
{
    public class StartProcessAndSendTcpData : ITuringMachineAgent
    {
        bool ConnectedOk;
        /// <summary>
        /// Process
        /// </summary>
        public ProcessStartInfoEx Process { get; set; }
        /// <summary>
        /// Connect to
        /// </summary>
        public IPEndPoint ConnectTo { get; set; }
        /// <summary>
        /// Connect timeout
        /// </summary>
        public TimeSpan ConnectTimeout { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StartProcessAndSendTcpData()
        {
            ConnectTimeout = TimeSpan.FromSeconds(30);
            ConnectedOk = false;
        }
        /// <summary>
        /// Create process
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        public override ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e)
        {
            return new WERDetector(Process);
        }
        public override void OnRun(TuringSocket socket, TuringAgentArgs e)
        {
            // Create client
            using (TcpClient ret = new TcpClient())
            {
                // Try connect to server
                IAsyncResult result = ret.BeginConnect(ConnectTo.Address, ConnectTo.Port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(ConnectTimeout);
                ret.EndConnect(result);

                if (!success) return;

                // Flag as connected
                ConnectedOk = true;

                // Fuzzer stream
                using (TuringStream stream = new TuringStream(socket))
                    try
                    {
                        //Try send all we can
                        using (Stream sr = ret.GetStream())
                        {
                            stream.CopyTo(sr);
                            sr.Flush();
                        }
                    }
                    catch //(Exception e)
                    {
                    }
            }
        }
        /// <summary>
        /// Check if can reconnect (¿its alive?)
        /// </summary>
        /// <param name="socket">Socket</param>
        public override bool GetItsAlive(TuringSocket socket, TuringAgentArgs e)
        {
            try
            {
                using (TcpClient ret = new TcpClient())
                {
                    ret.Connect(ConnectTo);
                    //IAsyncResult result = ret.BeginConnect(ConnectTo.Address, ConnectTo.Port, null, null);
                    //bool success = result.AsyncWaitHandle.WaitOne(ConnectTimeout);
                    //ret.EndConnect(result);

                    //if (success)
                    return true;
                }
            }
            catch
            {
            }
            // If sometime connected, and now not, are dead
            return !ConnectedOk;
        }
    }
}