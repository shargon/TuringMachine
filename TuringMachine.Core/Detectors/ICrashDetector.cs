using System;
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
        /// <param name="crashData">Crash data</param>
        /// <param name="crashExtension">Crash extension</param>
        /// <param name="isAlive">Its alive</param>
        public virtual bool IsCrashed(TuringSocket socket, out byte[] crashData, out string crashExtension, ITuringMachineAgent.delItsAlive isAlive)
        {
            throw new NotImplementedException();
        }
    }
}