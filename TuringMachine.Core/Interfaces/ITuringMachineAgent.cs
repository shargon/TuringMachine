﻿using System;
using TuringMachine.Core.Detectors;
using TuringMachine.Core.Sockets;

namespace TuringMachine.Core.Interfaces
{
    public class ITuringMachineAgent
    {
        public delegate bool delItsAlive(TuringSocket socket);

        /// <summary>
        /// Constructor
        /// </summary>
        protected ITuringMachineAgent() { }

        /// <summary>
        /// Create detector (First action)
        /// </summary>
        /// <param name="socket">Socket</param>
        public virtual ICrashDetector GetCrashDetector(TuringSocket socket)
        {
            return null;
        }
        /// <summary>
        /// Get Task of agent
        /// </summary>
        /// <param name="socket">Socket</param>
        public virtual void OnRun(TuringSocket socket)
        {
            throw (new NotImplementedException());
        }
        /// <summary>
        /// Return if its alive
        /// </summary>
        /// <param name="socket">Socket</param>
        public virtual bool GetItsAlive(TuringSocket socket)
        {
            return false;
        }
    }
}