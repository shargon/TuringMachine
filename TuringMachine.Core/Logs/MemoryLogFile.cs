using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Logs
{
    public class MemoryLogFile : ILogFile
    {
        string _FileName;
        byte[] _Data;

        public MemoryLogFile(string fileName, byte[] data)
        {
            _FileName = fileName;
            _Data = data;
        }

        public override byte[] Data { get { return _Data; } }
        public override string FileName { get { return _FileName; } }
    }
}