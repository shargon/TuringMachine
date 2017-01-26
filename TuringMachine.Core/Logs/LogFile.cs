using System;
using System.IO;
using System.Threading;
using TuringMachine.Core.Helpers;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Logs
{
    public class LogFile : ILogFile
    {
        string _Path, _FileName;
        byte[] _Data;
        TimeSpan _EnsureFileTimeout;

        public override byte[] Data
        {
            get
            {
                if (_Data != null) return _Data;

                Stream fread = WaitOpenRead(_Path, _EnsureFileTimeout);
                if (fread == null) return null;

                using (fread)
                {
                    _Data = new byte[fread.Length];
                    StreamHelper.ReadFull(fread, _Data, 0, _Data.Length);
                    return _Data;
                }
            }
        }

        public string Path { get { return _Path; } }
        public override string FileName { get { return _FileName; } }
        public TimeSpan EnsureFileTimeout { get { return _EnsureFileTimeout; } }

        public LogFile(string path, TimeSpan ensureFileTimeout)
        {
            _Path = path;
            _FileName = System.IO.Path.GetFileName(path);
            _EnsureFileTimeout = ensureFileTimeout;
        }


        /// <summary>
        /// Wait for read
        /// </summary>
        /// <param name="fileName">File</param>
        /// <param name="ensureFileTimeout">EnsureFile</param>
        static Stream WaitOpenRead(string fileName, TimeSpan ensureFileTimeout)
        {
            while (true)
            {
                if (!File.Exists(fileName))
                {
                    if (ensureFileTimeout != TimeSpan.Zero)
                    {
                        Thread.Sleep(ensureFileTimeout);
                        if (!File.Exists(fileName)) return null;
                    }
                    else return null;
                }

                try
                {
                    return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch { }
            }
        }
    }
}