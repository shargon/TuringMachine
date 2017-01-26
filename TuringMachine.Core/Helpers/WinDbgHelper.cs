using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TuringMachine.Core.Enums;

namespace TuringMachine.Core.Helpers
{
    /// <summary>
    /// Config at: 
    ///     http://insidetrust.blogspot.com.es/2011/02/assessing-buffer-overflows-with-windbg.html
    /// WindDbg:
    ///     https://developer.microsoft.com/es-es/windows/downloads/windows-10-sdk
    ///     copy to C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\winext
    /// !exploitable
    ///     http://msecdbg.codeplex.com/
    /// </summary>
    public class WinDbgHelper
    {
        /// <summary>
        /// @"C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\windbg.exe"
        /// </summary>
        public static string WinDbgPath { get; set; }

        const string check = "Exploitability Classification: ";

        /// <summary>
        /// Check dump Path
        /// </summary>
        /// <param name="dump">Dump</param>
        public static EExploitableResult CheckMemoryDump(string dump, out string logFile)
        {
            logFile = null;
            if (string.IsNullOrEmpty(WinDbgPath)) return EExploitableResult.NOT_WINDBG_CONFIGURED;
            if (!File.Exists(dump)) return EExploitableResult.NOT_DUMP_FOUND;

            string file = dump + ".~explog";

            try
            {
                using (Process p = Process.Start(new ProcessStartInfo()
                {
                    FileName = WinDbgPath,
                    // Load library, run !exploitable, Quit
                    Arguments = "-z \"" + dump + "\" -c \"!load winext\\msec.dll;!exploitable;q\" -logo \"" + file + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    //UseShellExecute = false,
                }))
                    p.WaitForExit();
            }
            catch
            {
                return EExploitableResult.NOT_WINDBG_CONFIGURED;
            }

            try
            {
                if (File.Exists(file))
                {
                    logFile = File.ReadAllText(file);
                    File.Delete(file);
                }
            }
            catch { }

            if (!string.IsNullOrEmpty(logFile))
            {
                string line = logFile.Replace("\r", "").Split('\n').Where(u => u.StartsWith(check)).FirstOrDefault();

                if (!string.IsNullOrEmpty(line))
                {
                    line = line.Substring(check.Length).Trim();

                    EExploitableResult r;
                    if (Enum.TryParse(line, out r)) return r;
                }
            }

            return EExploitableResult.ERROR_CHECKING;
        }
    }
}