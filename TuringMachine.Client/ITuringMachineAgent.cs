using System;
using TuringMachine.Client.Detectors;
using TuringMachine.Client.Sockets;

namespace TuringMachine.Client
{
    public class ITuringMachineAgent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ITuringMachineAgent() { }

        /// <summary>
        /// Get Task of agent
        /// </summary>
        /// <param name="socket">Socket</param>
        public virtual ICrashDetector Run(TuringSocket socket)
        {
            throw (new NotImplementedException());
        }
    }
}