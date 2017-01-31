using System;
using System.IO;
using System.Threading;
using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Core.Logs
{
    public class LogFile : ILogFile
    {
        string _Path, _FileName;
        byte[] _Data;

        /// <summary>
        /// Path
        /// </summary>
        public string Path { get { return _Path; } }
        /// <summary>
        /// Filename
        /// </summary>
        public override string FileName { get { return _FileName; } }
        /// <summary>
        /// Data
        /// </summary>
        public override byte[] Data { get { return _Data; } }

        public LogFile(string path)
        {
            _Path = path;
            _FileName = System.IO.Path.GetFileName(path);
        }

        /// <summary>
        /// Try load the file
        /// </summary>
        /// <param name="timeOut">Timeout</param>
        public bool TryLoadFile(TimeSpan timeOut)
        {
            _Data = ReadFileWaiting(_Path, timeOut);
            return _Data != null;
        }
        /// <summary>
        /// Blocks until the file is not locked any more
        /// </summary>
        /// <param name="fullPath">Full path</param>
        /// <param name="ensureFileTimeout">Wait time if the file not exists</param>
        public static byte[] ReadFileWaiting(string fullPath, TimeSpan ensureFileTimeout)
        {
            double sec = ensureFileTimeout.TotalMilliseconds;

            while (true)
            {
                try
                {
                    // Attempt to open the file exclusively.
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                    {
                        byte[] data = new byte[fs.Length];
                        StreamHelper.ReadFull(fs, data, 0, data.Length);
                        return data;
                    }
                }
                catch //(Exception ex)
                {
                    if (sec <= 0) return null;

                    // Wait for the lock to be released
                    Thread.Sleep(250);

                    if (!File.Exists(fullPath))
                        sec -= 250;
                }
            }
        }
    }
}