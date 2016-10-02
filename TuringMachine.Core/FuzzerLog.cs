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
        public IPEndPoint Origin { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public ETestResult Type { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; set; }
    }
}