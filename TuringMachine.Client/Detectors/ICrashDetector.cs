using System;
using TuringMachine.Client.Sockets;

namespace TuringMachine.Client.Detectors
{
    public class ICrashDetector
    {
        protected ICrashDetector() { }

        /// <summary>
        /// Receive alive signal
        /// </summary>
        /// <param name="isAlive">Its alive</param>
        public virtual void SetIsAlive(bool isAlive) { }
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