using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using TuringMachine.Core.Interfaces;
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
        public override bool IsCrashed(TuringSocket socket, out byte[] zipCrashData, ITuringMachineAgent.delItsAlive isAlive)
        {
            zipCrashData = null;

            if (_Process == null) return false;

            Thread.Sleep(500);

            // Check store location
            bool isBreak = GetStoreLocation() != _StoreLocation;

            // Search file 
            if (!isBreak)
                foreach (string file in _FileNames)
                    if (File.Exists(file)) { isBreak = true; break; }

            // Wait if break detection
            if (isBreak) Thread.Sleep(5000);

            // If its alive kill them
            if (isAlive == null || isAlive.Invoke(socket))
            {
                foreach (Process p in _Process)
                    try { p.Kill(); }
                    catch { }
            }

            // Wait for exit
            foreach (Process p in _Process)
                try { p.WaitForExit(); } catch { }

            // Compress to zip
            byte[] zip = null;
            if (ZipHelper.AppendOrCreateZip(ref zip, GetDumpFiles(_FileNames, isBreak)) > 0 && zip != null && zip.Length > 0)
                zipCrashData = zip;

            return zipCrashData != null && zipCrashData.Length > 0;
        }
        IEnumerable<ZipHelper.FileEntry> GetDumpFiles(string[] fileNames, bool isBreak)
        {
            foreach (string file in fileNames)
            {
                Stream fread = WaitOpenRead(file, isBreak);
                if (fread == null)
                    continue;

                byte[] data;
                using (fread)
                {
                    data = new byte[fread.Length];
                    StreamHelper.ReadFull(fread, data, 0, data.Length);
                }

                yield return new ZipHelper.FileEntry(Path.GetFileName(file), data);
            }
        }
        /// <summary>
        /// Wait for read
        /// </summary>
        /// <param name="fileName">File</param>
        /// <param name="ensureFile">EnsureFile</param>
        Stream WaitOpenRead(string fileName, bool ensureFile)
        {
            while (true)
            {
                if (!File.Exists(fileName))
                {
                    if (ensureFile)
                    {
                        Thread.Sleep(1000);
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