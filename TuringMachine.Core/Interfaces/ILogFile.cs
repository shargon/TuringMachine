using TuringMachine.Helpers;

namespace TuringMachine.Core.Interfaces
{
    public class ILogFile
    {
        /// <summary>
        /// Data
        /// </summary>
        public virtual byte[] Data { get; }
        /// <summary>
        /// FileName
        /// </summary>
        public virtual string FileName { get; }

        /// <summary>
        /// Get entry
        /// </summary>
        public ZipHelper.FileEntry GetZipEntry()
        {
            return new ZipHelper.FileEntry(FileName, Data);
        }
    }
}