using System.Diagnostics;
using System.IO;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class ExecutionFuzzingInput : IFuzzingInput
    {
        /// <summary>
        /// Name
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// Args
        /// </summary>
        public string Arguments { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Execution"; } }
        /// <summary>
        /// IsSelectable
        /// </summary>
        public bool IsSelectable { get { return true; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filename">File</param>
        /// <param name="args">Args</param>
        public ExecutionFuzzingInput(string filename, string args)
        {
            FileName = filename;
            Arguments = args;
        }
        /// <summary>
        /// Get process stream
        /// </summary>
        public Stream GetStream()
        {
            Process pr = Process.Start(new ProcessStartInfo()
            {
                FileName = FileName,
                Arguments = Arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            return pr.StandardOutput.BaseStream;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return FileName + (string.IsNullOrEmpty(Arguments) ? "" : " \"" + Arguments + "\"");
        }
    }
}