using System.Collections.Generic;
using System.Diagnostics;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets;
using TuringMachine.Helpers;
using TuringMachine.Helpers.Enums;

namespace TuringMachine.Core.Detectors.Multi
{
    public class OutputErrorProcessDetector : ICrashDetector
    {
        Process[] _Process;
        /// <summary>
        /// Process
        /// </summary>
        public Process[] Process
        {
            get { return _Process; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pis">Process start info</param>
        public OutputErrorProcessDetector(params ProcessStartInfo[] pis)
        {
            List<Process> pr = new List<Process>();

            foreach (ProcessStartInfo pi in pis)
            {
                pi.UseShellExecute = false;
                pi.RedirectStandardInput = true;
                pi.RedirectStandardError = true;

                Process prr = System.Diagnostics.Process.Start(pi);
                if (prr != null)
                    pr.Add(prr);
            }

            _Process = pr.ToArray();
        }

        public override bool IsCrashed(TuringSocket socket, out byte[] zipCrashData, out EExploitableResult exploitResult, ITuringMachineAgent.delItsAlive isAlive, TuringAgentArgs e)
        {
            byte[] zip = null;
            zipCrashData = null;
            exploitResult = EExploitableResult.NOT_DUMP_FOUND;

            List<ZipHelper.FileEntry> errors = new List<ZipHelper.FileEntry>();

            foreach (Process pr in _Process)
            {
                if (!pr.HasExited) continue;

                string error = pr.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(error.Trim()))
                    errors.Add
                        (
                        new ZipHelper.FileEntry("Error_" + pr.ProcessName + "_" + pr.Id.ToString() + ".txt",
                        pr.StandardError.CurrentEncoding.GetBytes(error.Trim()))
                        );
            }

            if (errors.Count > 0)
            {
                if (ZipHelper.AppendOrCreateZip(ref zip, errors) > 0 && zip != null && zip.Length > 0)
                    zipCrashData = zip;
            }

            return zipCrashData != null && zipCrashData.Length > 0;
        }
    }
}