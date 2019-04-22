using System;
using System.Collections.Generic;
using System.Dynamic;

namespace TuringMachine.Core.Arguments
{
    public class TuringAgentArgs
    {
        /// <summary>
        /// Task id
        /// </summary>
        public Guid TaskId { get; private set; }

        /// <summary>
        /// Variables
        /// </summary>
        public IDictionary<string, object> Vars { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TuringAgentArgs()
        {
            TaskId = Guid.NewGuid();
        }
    }
}