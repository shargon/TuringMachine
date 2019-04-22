using TuringMachine.Core.Arguments;
using TuringMachine.Core.Delegates;
using TuringMachine.Core.Sockets;
using TuringMachine.Helpers.Enums;

namespace TuringMachine.Core.Detectors
{
    public interface ICrashDetector
    {
        /// <summary>
        /// Return crashed data
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="zipCrashData">Crash data</param>
        /// <param name="exploitResult">Explotation result</param>
        /// <param name="isAlive">Its alive</param>
        /// <param name="e">Arguments</param>
        bool IsCrashed(TuringSocket socket, out byte[] zipCrashData, out EExploitableResult exploitResult, delItsAlive isAlive, TuringAgentArgs e);
    }
}