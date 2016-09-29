using System.Collections.Generic;
using TuringMachine.Core.Mutational.Changes;

namespace TuringMachine.Core.Mutational
{
    public class MutationalOffset : Offset
    {
        /// <summary>
        /// Changes
        /// </summary>
        public List<IMutationalChange> Changes { get; set; }
    }
}