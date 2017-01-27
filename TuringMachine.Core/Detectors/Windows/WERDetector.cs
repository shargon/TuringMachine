using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Helpers;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Logs;
using TuringMachine.Core.Sockets;
using TuringMachine.Helpers;

namespace TuringMachine.Core.Detectors.Windows
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
        string _StoreLocation;
        RegistryView _View;

        public event EventHandler OnDispose;

        /// <summary>
        /// Exit timeout
        /// </summary>
        public TimeSpan ExitTimeout { get; set; }

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

                    if (Is64Bit(p.Handle)) _View = RegistryView.Registry64;
                    else _View = RegistryView.Registry32;

                    lp.Add(p);
                    ls.Add(Path.Combine(_CrashPath, file + "." + p.Id.ToString() + ".dmp"));
                }

                _FileNames = ls.ToArray();
                _Process = lp.ToArray();
                _StoreLocation = GetStoreLocation();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);
        static bool Is64Bit(IntPtr handle)
        {
            bool retVal;
            IsWow64Process(handle, out retVal);
            return retVal;
        }
        /// <summary>
        /// Check in Store location (its called first)
        /// </summary>
        string GetStoreLocation()
        {
            try
            {
                // Read path
                using (RegistryKey r = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, _View).OpenSubKey(@"SOFTWARE\Microsoft\Windows\Windows Error Reporting\Debug", false))
                    return r.GetValue("StoreLocation", "").ToString();
            }
            catch { }
            return null;
        }
        /// <summary>
        /// Return if are WER file
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="zipCrashData">Crash data</param>
        /// <param name="isAlive">IsAlive</param>
        public override bool IsCrashed(TuringSocket socket, out byte[] zipCrashData, out EExploitableResult exploitResult, ITuringMachineAgent.delItsAlive isAlive, TuringAgentArgs e)
        {
            zipCrashData = null;

            if (_Process == null)
            {
                exploitResult = EExploitableResult.NOT_DUMP_FOUND;
                return false;
            }

            // Wait for exit
            foreach (Process p in _Process)
                try { p.WaitForExit((int)ExitTimeout.TotalMilliseconds); } catch { }

            // Courtesy wait
            Thread.Sleep(500);

            // Check store location for changes
            bool isBreak = GetStoreLocation() != _StoreLocation;

            // Search logs 
            List<ILogFile> fileAppend = new List<ILogFile>();

            if (_FileNames != null)
                foreach (string f in _FileNames)
                {
                    LogFile l = new LogFile(f);

                    if (l.TryLoadFile(TimeSpan.FromSeconds(isBreak ? 5 : 2)))
                        fileAppend.Add(l);
                }

            // If its alive kill them
            if (isAlive == null || isAlive.Invoke(socket, e))
            {
                foreach (Process p in _Process)
                    try { p.Kill(); } catch { }
            }

            // Check exploitability
            exploitResult = EExploitableResult.NOT_DUMP_FOUND;
            for (int x = 0, m = fileAppend.Count; x < m; x++)
            {
                LogFile dump = (LogFile)fileAppend[x];

                if (dump.FileName.ToLowerInvariant().EndsWith(".dmp"))
                {
                    string log;
                    exploitResult = WinDbgHelper.CheckMemoryDump(dump.Path, out log);
                    if (!string.IsNullOrEmpty(log))
                        fileAppend.Add(new MemoryLogFile("exploitable.log", Encoding.UTF8.GetBytes(log)));
                }
            }

            // Compress to zip
            byte[] zip = null;
            if (ZipHelper.AppendOrCreateZip(ref zip, fileAppend.Select(u => u.GetZipEntry())) > 0 && zip != null && zip.Length > 0)
                zipCrashData = zip;

            return zipCrashData != null && zipCrashData.Length > 0;
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

            OnDispose?.Invoke(null, null);
        }
    }
}