using System.IO;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class FileFuzzingInput : IFuzzingInput
    {
        byte[] _Data;
        FileInfo _Info;
        /// <summary>
        /// Use Cache
        /// </summary>
        public bool UseCache { get; set; }
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
            _Info = new FileInfo(filename);
            UseCache = true;
        }
        /// <summary>
        /// Get file stream
        /// </summary>
        public byte[] GetStream()
        {
            if (!UseCache) return File.ReadAllBytes(FileName);

            if (_Data != null)
            {
                // Check Cache state
                _Info.Refresh();
                if (_Data.Length == _Info.Length)
                    return _Data;
            }

            _Data = File.ReadAllBytes(FileName);
            return _Data;
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