using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

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
        string[] _FileNames;
        static string _CrashPath;
        Process[] _Process;

        /// <summary>
        /// Wait process End
        /// </summary>
        bool KillProcess { get; set; }

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
        /// Receive is alive signal
        /// </summary>
        /// <param name="isAlive">Alive</param>
        public void IsAliveSignal(bool isAlive)
        {
            if (!isAlive)
                KillProcess = false;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startInfo">Process startInfo</param>
        public WERDetector(params ProcessStartInfo[] startInfo)
        {
            if (startInfo != null)
            {
                List<string> ls = new List<string>();
                List<Process> lp = new List<Process>();

                foreach (ProcessStartInfo pi in startInfo)
                {
                    string file = Path.GetFileName(pi.FileName);

                    Process p = Process.Start(pi);
                    lp.Add(p);
                    ls.Add(Path.Combine(_CrashPath, file + "." + p.Id.ToString() + ".dmp"));
                }

                _FileNames = ls.ToArray();
                _Process = lp.ToArray();
            }

            KillProcess = true;
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

            if (KillProcess)
            {
                // Kill
                foreach (Process p in _Process)
                    try { p.Kill(); } catch { }
            }

            // Wait
            foreach (Process p in _Process)
                try { p.WaitForExit(); } catch { }

            // Compress to zip
            using (MemoryStream memoryStream = new MemoryStream())
            {
                foreach (string file in _FileNames)
                {
                    Stream fread = WaitOpenRead(file);
                    if (fread == null) continue;

                    using (fread)
                    using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        using (Stream entryStream = archive.CreateEntry(Path.GetFileName(file)).Open())
                            fread.CopyTo(entryStream);
                    }
                }

                // Recover zip
                memoryStream.Seek(0, SeekOrigin.Begin);

                crashData = memoryStream.ToArray();
                if (crashData != null && crashData.Length > 0) crashExtension = "zip";
                else crashData = null;

                return !string.IsNullOrEmpty(crashExtension);
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
                foreach (Process p in _Process)
                    try { p.Dispose(); } catch { }

                _Process = null;
            }
        }
    }
}