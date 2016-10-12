using System.IO;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class FileFuzzingInput : IFuzzingInput
    {
        /// <summary>
        /// Name
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "File"; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filename">File</param>
        public FileFuzzingInput(string filename)
        {
            FileName = filename;
        }
        /// <summary>
        /// Get file stream
        /// </summary>
        public byte[] GetStream()
        {
            return File.ReadAllBytes(FileName);
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return FileName;
        }
    }
}