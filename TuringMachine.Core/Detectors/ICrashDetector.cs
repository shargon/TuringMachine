using System;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets;

namespace TuringMachine.Core.Detectors
{
    public class ICrashDetector
    {
        protected ICrashDetector() { }

        /// <summary>
        /// Return crashed data
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="zipCrashData">Crash data</param>
        /// <param name="exploitResult">Explotation result</param>
        /// <param name="isAlive">Its alive</param>
        /// <param name="e">Arguments</param>
        public virtual bool IsCrashed(TuringSocket socket, out byte[] zipCrashData, out EExploitableResult exploitResult, ITuringMachineAgent.delItsAlive isAlive, TuringAgentArgs e)
        {
            throw new NotImplementedException();
        }
    }
}