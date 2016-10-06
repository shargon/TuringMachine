using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace TuringMachine.Client.Detectors.Windows
{
    /// <summary>
    ///         LocalMode: https://msdn.microsoft.com/es-es/library/windows/desktop/bb787181(v=vs.85).aspx
    /// RegEdit (x32 x64): https://support.microsoft.com/en-us/kb/305097
    /// 
    ///             Windows Registry Editor Version 5.00
    ///             [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps]
    ///             "DumpType"=dword:00000002
    ///             "LocalDumps"=dword:00001000
    ///             "DumpFolder"="c:\\CrashDumps"
    ///             
    /// </summary>
    public class WERDetector : ICrashDetector, IDisposable
    {
        string _FileName;
        static string _CrashPath;
        Process _Process;

        /// <summary>
        /// Load CrashPath
        /// </summary>
        static WERDetector()
        {
            _CrashPath = @"%LOCALAPPDATA%\CrashDumps";

            try
            {
                // Read path
                using (RegistryKey r = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps", false))
                {
                    if (r != null)
                    {
                        string v = r.GetValue("DumpFolder", _CrashPath).ToString();
                        if (!string.IsNullOrEmpty(v))
                            _CrashPath = v;
                    }
                }
            }
            catch { }

            _CrashPath = Environment.ExpandEnvironmentVariables(_CrashPath);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pid">Process id</param>
        public WERDetector(int pid) : this(Process.GetProcessById(pid)) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">File</param>
        /// <param name="arguments">Arguments</param>
        public WERDetector(string fileName, string arguments) : this(Process.Start(fileName, arguments)) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="process">Process (dont release the class!)</param>
        public WERDetector(Process process)
        {
            if (process != null)
            {
                string file = Path.GetFileName(process.MainModule.FileName);
                _FileName = Path.Combine(_CrashPath, file + "." + process.Id.ToString() + ".dmp");
            }

            _Process = process;
        }
        /// <summary>
        /// Return if are WER file
        /// </summary>
        /// <param name="crashData">Crash data</param>
        /// <param name="crashExtension">Crash extension</param>
        public override bool IsCrashed(out byte[] crashData, out string crashExtension)
        {
            crashData = null;
            crashExtension = null;

            if (_Process == null) return false;

            _Process.WaitForExit();
            //Thread.Sleep(1000);

            Stream file = WaitOpenRead(_FileName);
            if (file == null) return false;

            // Compress to zip
            using (file)
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    using (Stream entryStream = archive.CreateEntry(Path.GetFileName(_FileName)).Open())
                        file.CopyTo(entryStream);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                crashExtension = "zip";
                crashData = memoryStream.ToArray();
                return true;
            }
        }
        /// <summary>
        /// Wait for read
        /// </summary>
        /// <param name="fileName">File</param>
        Stream WaitOpenRead(string fileName)
        {
            while (true)
            {
                if (!File.Exists(fileName)) return null;

                try
                {
                    return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch { }
            }
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            if (_Process != null)
            {
                _Process.Dispose();
                _Process = null;
            }
        }
    }
}