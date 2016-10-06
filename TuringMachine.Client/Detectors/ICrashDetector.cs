using System;

namespace TuringMachine.Client.Detectors
{
    public class ICrashDetector
    {
        protected ICrashDetector() { }
        
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