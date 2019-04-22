using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Delegates;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Logs;
using TuringMachine.Core.Sockets;
using TuringMachine.Helpers;
using TuringMachine.Helpers.Enums;

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
        class iProcess : IDisposable
        {
            public ProcessStartInfoEx StartInfo { get; private set; }

            ServiceController Service;
            Process Process;
            readonly string StoreLocation32;
            readonly string StoreLocation64;

            public bool HasExited
            {
                get
                {
                    if (Process != null) return Process.HasExited;
                    if (Service != null) return Service.Status != ServiceControllerStatus.Running;

                    return true;
                }
            }

            public iProcess(Process p, ProcessStartInfoEx pi)
            {
                Process = p;
                StartInfo = pi;

                StoreLocation64 = GetStoreLocation(RegistryView.Registry64);
                StoreLocation32 = GetStoreLocation(RegistryView.Registry32);
            }
            public iProcess(ServiceController s, ProcessStartInfoEx pi)
            {
                Service = s;
                StartInfo = pi;

                StoreLocation64 = GetStoreLocation(RegistryView.Registry64);
                StoreLocation32 = GetStoreLocation(RegistryView.Registry32);
            }
            public bool ItsChangedStoreLocation()
            {
                if (GetStoreLocation(RegistryView.Registry64) != StoreLocation64) return true;
                if (GetStoreLocation(RegistryView.Registry32) != StoreLocation32) return true;
                return false;
            }
            public void WaitForExit(int totalMilliseconds)
            {
                if (Process != null) Process.WaitForExit(totalMilliseconds);
            }
            /// <summary>
            /// Check in Store location (its called first)
            /// </summary>
            static string GetStoreLocation(RegistryView v)
            {
                try
                {
                    // Read path
                    using (var r = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, v).OpenSubKey(@"SOFTWARE\Microsoft\Windows\Windows Error Reporting\Debug", false))
                    {
                        return r.GetValue("StoreLocation", "").ToString();
                    }
                }
                catch { }
                return null;
            }
            public void Dispose()
            {
                if (Process != null)
                {
                    Process.Dispose();
                    Process = null;
                }
                if (Service != null)
                {
                    Service.Dispose();
                    Service = null;
                }
            }
            public void KillProcess()
            {
                if (Process != null) Process.Kill();
            }
        }

        readonly string[] _FileNames;
        readonly static string _CrashPath;
        iProcess[] _Process;

        public event EventHandler OnDispose;

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
        public WERDetector(params ProcessStartInfoEx[] startInfo)
        {
            if (startInfo != null)
            {
                var files = new List<string>();
                var process = new List<iProcess>();

                foreach (var pi in startInfo)
                {
                    if (pi == null) continue;

                    var file = "";
                    var pid = 0;
                    if (!string.IsNullOrEmpty(pi.FileName))
                    {
                        file = Path.GetFileName(pi.FileName);

                        var p = Process.Start(pi.GetProcessStartInfo());
                        pid = p.Id;
                        process.Add(new iProcess(p, pi));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(pi.ServiceName))
                        {
                            var controller = new ServiceController(pi.ServiceName);
                            if (controller.Status == ServiceControllerStatus.Stopped)
                            {
                                controller.Start();
                                controller.WaitForStatus(ServiceControllerStatus.Running, pi.ExitTimeout);
                            }

                            try
                            {
                                using (var service = new ManagementObject(@"Win32_service.Name='" + pi.ServiceName + "'"))
                                {
                                    var o = service.GetPropertyValue("ProcessId");
                                    pid = Convert.ToInt32(o);

                                    file = ((string)service.GetPropertyValue("PathName")).Trim();

                                    if (File.Exists(file))
                                        file = Path.GetFileName(file);
                                    else
                                    {
                                        if (file.StartsWith("\""))
                                        {
                                            file = file.Substring(1);

                                            var ix = file.IndexOf("\"");
                                            if (ix < 0) file = null;
                                            else
                                            {
                                                file = file.Substring(0, ix);
                                                if (File.Exists(file))
                                                    file = Path.GetFileName(file);
                                                else file = null;
                                            }
                                        }
                                        else file = null;
                                    }
                                }

                                process.Add(new iProcess(controller, pi));
                            }
                            catch { }
                        }
                    }

                    if (!string.IsNullOrEmpty(file) && pid != 0 && pi.WaitMemoryDump)
                        files.Add(Path.Combine(_CrashPath, file + "." + pid.ToString() + ".dmp"));
                }

                _FileNames = files.ToArray();
                _Process = process.ToArray();
            }
        }
        /// <summary>
        /// Return if are WER file
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="zipCrashData">Crash data</param>
        /// <param name="isAlive">IsAlive</param>
        public bool IsCrashed(TuringSocket socket, out byte[] zipCrashData, out EExploitableResult exploitResult, delItsAlive isAlive, TuringAgentArgs e)
        {
            zipCrashData = null;

            if (_Process == null)
            {
                exploitResult = EExploitableResult.NOT_DUMP_FOUND;
                return false;
            }

            // Wait for exit
            var isBreak = false;

            if (_Process != null)
            {
                var miniwait = TimeSpan.FromMilliseconds(100);
                foreach (var p in _Process)
                {
                    try
                    {
                        var ts = TimeSpan.FromMilliseconds(p.StartInfo.ExitTimeout.TotalMilliseconds);
                        while (ts.TotalMilliseconds > 0 &&
                            (isAlive == null || isAlive.Invoke(socket, e)) && !p.HasExited)
                        {
                            p.WaitForExit((int)miniwait.TotalMilliseconds);
                            ts = ts.Subtract(miniwait);
                        }
                    }
                    catch { }

                    // Check store location for changes
                    if (!isBreak && p.ItsChangedStoreLocation())
                        isBreak = true;
                }
            }

            // Courtesy wait
            Thread.Sleep(500);

            // Search logs 
            var fileAppend = new List<ILogFile>();

            if (_FileNames != null)
            {
                foreach (var f in _FileNames)
                {
                    var l = new LogFile(f);

                    if (l.TryLoadFile(TimeSpan.FromSeconds(isBreak ? 5 : 2)))
                        fileAppend.Add(l);
                }
            }

            // If its alive kill them
            if (_Process != null)
            {
                foreach (var p in _Process)
                    try { p.KillProcess(); } catch { }
            }

            // Check exploitability
            exploitResult = EExploitableResult.NOT_DUMP_FOUND;
            for (int x = 0, m = fileAppend.Count; x < m; x++)
            {
                var dump = (LogFile)fileAppend[x];

                if (dump.FileName.ToLowerInvariant().EndsWith(".dmp"))
                {
                    exploitResult = WinDbgHelper.CheckMemoryDump(dump.Path, out string log);
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
                foreach (var p in _Process)
                {
                    try { p.Dispose(); } catch { }
                }

                _Process = null;
            }

            OnDispose?.Invoke(null, null);
        }
    }
}