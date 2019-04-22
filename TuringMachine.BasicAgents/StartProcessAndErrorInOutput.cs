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

        public bool GetItsAlive(TuringSocket socket, TuringAgentArgs e) => false;

        /// <summary>
        /// Create process
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        public ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e)
        {
            var ret = new Core.Detectors.Multi.OutputErrorProcessDetector(Process.Select(u => u.GetProcessStartInfo()).ToArray());

            socket[ProcessPid] = ret;
            return ret;
        }

        public void OnRun(TuringSocket socket, TuringAgentArgs e)
        {
            var ret = (Core.Detectors.Multi.OutputErrorProcessDetector)socket[ProcessPid];

            // Fuzzer stream
            using (var stream = new TuringStream(socket))
                try
                {
                    var data = new byte[4096];
                    var cdata = new char[data.Length];

                    // Try send all we can

                    while (true)
                    {
                        var r = stream.Read(data, 0, data.Length);

                        if (r > 0)
                        {
                            for (var x = 0; x < r; x++) cdata[x] = (char)data[x];

                            foreach (var process in ret.Process)
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