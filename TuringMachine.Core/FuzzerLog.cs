using System;
using System.Net;
using TuringMachine.Core.Enums;

namespace TuringMachine.Core
{
    public class FuzzerLog
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Origin
        /// </summary>
        public IPAddress Origin { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public EFuzzingReturn Type { get; set; }
        /// <summary>
        /// Input
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// Config
        /// </summary>
        public string Config { get; set; }
        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FuzzerLog()
        {
            Date = DateTime.Now;
        }
    }
}