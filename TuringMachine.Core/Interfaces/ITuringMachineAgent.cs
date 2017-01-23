using System;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Detectors;
using TuringMachine.Core.Sockets;

namespace TuringMachine.Core.Interfaces
{
    public class ITuringMachineAgent
    {
        public delegate bool delItsAlive(TuringSocket socket, TuringAgentArgs e);

        /// <summary>
        /// Constructor
        /// </summary>
        protected ITuringMachineAgent() { }

        /// <summary>
        /// Create detector (First action)
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        public virtual ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e)
        {
            return null;
        }
        /// <summary>
        /// Get Task of agent
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        public virtual void OnRun(TuringSocket socket, TuringAgentArgs e)
        {
            throw (new NotImplementedException());
        }
        /// <summary>
        /// Return if its alive
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        public virtual bool GetItsAlive(TuringSocket socket, TuringAgentArgs e)
        {
            return false;
        }
        ///// <summary>
        ///// Free Task
        ///// </summary>
        ///// <param name="socket">Socket</param>
        ///// <param name="e">Arguments</param>
        //public virtual void OnFree(TuringSocket socket, TuringAgentArgs e) { }
        ///// <summary>
        ///// On Load
        ///// </summary>
        ///// <param name="socket">Socket</param>
        ///// <param name="e">Arguments</param>
        //public virtual void OnLoad(TuringSocket socket, TuringAgentArgs e) { }
    }
}