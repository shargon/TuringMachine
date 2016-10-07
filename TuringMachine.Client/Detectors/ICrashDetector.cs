using System;

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
        /// <param name="crashData">Crash data</param>
        /// <param name="crashExtension">Crash extension</param>
        public virtual bool IsCrashed(out byte[] crashData, out string crashExtension)
        {
            throw new NotImplementedException();
        }
    }
}