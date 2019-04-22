using TuringMachine.Core.Arguments;
using TuringMachine.Core.Detectors;
using TuringMachine.Core.Sockets;

namespace TuringMachine.Core.Interfaces
{
    public interface ITuringMachineAgent
    {
        /// <summary>
        /// Create detector (First action)
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e);

        /// <summary>
        /// Get Task of agent
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        void OnRun(TuringSocket socket, TuringAgentArgs e);

        /// <summary>
        /// Return if its alive
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        bool GetItsAlive(TuringSocket socket, TuringAgentArgs e);

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