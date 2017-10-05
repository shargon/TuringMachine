using System.Linq;
using TuringMachine.Core;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Detectors;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets;

namespace TuringMachine.BasicAgents
{
    public class StartProcessAndErrorInOutput : ITuringMachineAgent
    {
        const string ProcessPid = "PID";

        /// <summary>
        /// Process
        /// </summary>
        public ProcessStartInfoEx[] Process { get; set; }

        /// <summary>
        /// Create process
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        public override ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e)
        {
            Core.Detectors.Multi.OutputErrorProcessDetector ret = new 
                Core.Detectors.Multi.OutputErrorProcessDetector(Process.Select(u => u.GetProcessStartInfo()).ToArray());

            socket[ProcessPid] = ret;
            return ret;
        }
        public override void OnRun(TuringSocket socket, TuringAgentArgs e)
        {
            Core.Detectors.Multi.OutputErrorProcessDetector ret = (Core.Detectors.Multi.OutputErrorProcessDetector)socket[ProcessPid];

            // Fuzzer stream
            using (TuringStream stream = new TuringStream(socket))
                try
                {
                    byte[] data = new byte[4096];
                    char[] cdata = new char[data.Length];

                    // Try send all we can
                    while (true)
                    {
                        int r = stream.Read(data, 0, data.Length);

                        if (r > 0)
                        {
                            for (int x = 0; x < r; x++) cdata[x] = (char)data[x];

                            foreach (System.Diagnostics.Process process in ret.Process)
                            {
                                process.StandardInput.Write(cdata, 0, cdata.Length);
                                process.StandardInput.Flush();
                            }
                        }
                        else break;
                    }
                }
                catch //(Exception e)
                {
                }
        }
    }
}