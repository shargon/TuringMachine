using System;
using System.Diagnostics;
using System.Security;
using System.Text;

namespace TuringMachine.Core.Detectors
{
    public class ProcessStartInfoEx
    {
        public ProcessStartInfoEx() : this(null, null) { }
        public ProcessStartInfoEx(string fileName) : this(fileName, null) { }
        public ProcessStartInfoEx(string fileName, string arguments)
        {
            FileName = fileName;
            Arguments = arguments;

            CreateNoWindow = true;
            WindowStyle = ProcessWindowStyle.Hidden;
            UseShellExecute = false;
            WaitMemoryDump = true;
        }

        #region Public fields
        
        /// <summary>
        /// Exit timeout
        /// </summary>
        public TimeSpan ExitTimeout { get; set; }
        
        /// <summary>
        /// When exit process, wait for memory dump
        /// </summary>
        public bool WaitMemoryDump { get; set; }
        
        /// <summary>
        /// Service name
        /// </summary>
        public string ServiceName { get; set; }

        #endregion

        #region Base

        public string Arguments { get; set; }
        public bool CreateNoWindow { get; set; }
        public string Domain { get; set; }
        //public StringDictionary EnvironmentVariables { get; }
        public bool ErrorDialog { get; set; }
        public IntPtr ErrorDialogParentHandle { get; set; }
        public string FileName { get; set; }
        public bool LoadUserProfile { get; set; }
        public SecureString Password { get; set; }
        public bool RedirectStandardError { get; set; }
        public bool RedirectStandardInput { get; set; }
        public bool RedirectStandardOutput { get; set; }
        public Encoding StandardErrorEncoding { get; set; }
        public Encoding StandardOutputEncoding { get; set; }
        public string UserName { get; set; }
        public bool UseShellExecute { get; set; }
        public string Verb { get; set; }
        public string[] Verbs { get; }
        public ProcessWindowStyle WindowStyle { get; set; }
        public string WorkingDirectory { get; set; }

        #endregion

        public ProcessStartInfo GetProcessStartInfo()
        {
            return new ProcessStartInfo()
            {
                Arguments = Arguments,
                FileName = FileName,
                CreateNoWindow = CreateNoWindow,
                Domain = Domain,
                ErrorDialog = ErrorDialog,
                //EnvironmentVariables = EnvironmentVariables,
                ErrorDialogParentHandle = ErrorDialogParentHandle,
                LoadUserProfile = LoadUserProfile,
                Password = Password,
                RedirectStandardError = RedirectStandardError,
                RedirectStandardInput = RedirectStandardInput,
                RedirectStandardOutput = RedirectStandardOutput,
                StandardErrorEncoding = StandardErrorEncoding,
                StandardOutputEncoding = StandardOutputEncoding,
                UserName = UserName,
                UseShellExecute = UseShellExecute,
                Verb = Verb,
                WindowStyle = WindowStyle,
                WorkingDirectory = WorkingDirectory
            };
        }
        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (PropertyInfo pi in this.GetType().GetProperties())
        //    {
        //        object v = pi.GetValue(this);
        //        if (v == null) sb.Append(pi.Name.PadLeft(15, ' ') + ": NULL");
        //        else sb.Append(pi.Name.PadLeft(15, ' ') + ": " + v.ToString());
        //    }
        //    return sb.ToString();
        //}
    }
}
