using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Inputs;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Mutational;

namespace TuringMachine.Core
{
    /// <summary>
    /// [Client] <<----  Read Fuzz Information      <<----  [Server]  <<---- [Inputs]
    ///          ----->> Write Information for Fuz  ---------------------->>
    /// </summary>
    public class FuzzerServer
    {
        public delegate void delOnTestEnd(object sender, ETestResult result);
        public delegate void delOnCrashLog(object sender, FuzzerLog log);

        public event EventHandler OnInputsChange;
        public event EventHandler OnConfigurationsChange;
        public event EventHandler OnListenChange;

        public event delOnTestEnd OnTestEnd;
        public event delOnCrashLog OnCrashLog;

        List<FuzzerLog> _Logs;
        IPEndPoint _EndPoint = new IPEndPoint(IPAddress.Any, 7777);
        /// <summary>
        /// Inputs
        /// </summary>
        public List<FuzzerStat<IFuzzingInput>> Inputs { get; private set; }
        /// <summary>
        /// Configurations
        /// </summary>
        public List<FuzzerStat<IFuzzingConfig>> Configurations { get; private set; }
        /// <summary>
        /// Logs
        /// </summary>
        public FuzzerLog[] Logs { get { return _Logs.ToArray(); } }
        /// <summary>
        /// Listen address
        /// </summary>
        public IPEndPoint Listen
        {
            get { return _EndPoint; }
            set
            {
                _EndPoint = value;
                OnListenChange?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public FuzzerServer()
        {
            Inputs = new List<FuzzerStat<IFuzzingInput>>();
            Configurations = new List<FuzzerStat<IFuzzingConfig>>();
            _Logs = new List<FuzzerLog>();
        }
        /// <summary>
        /// Add input
        /// </summary>
        /// <param name="input">Input</param>
        public void AddInput(IFuzzingInput input)
        {
            if (Inputs == null) return;

            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
            OnInputsChange?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Add config
        /// </summary>
        /// <param name="file">File</param>
        public void AddConfig(string file)
        {
            if (string.IsNullOrEmpty(file)) return;
            if (!File.Exists(file)) return;

            switch (Path.GetExtension(file).ToLowerInvariant())
            {
                case ".fmut":
                    {
                        MutationConfig c = null;
                        try { c = MutationConfig.FromJson(File.ReadAllText(file, Encoding.UTF8)); } catch { }
                        if (c != null)
                        {
                            Configurations.Add(new FuzzerStat<IFuzzingConfig>(c));
                            OnConfigurationsChange?.Invoke(this, EventArgs.Empty);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Raise on crash log
        /// </summary>
        /// <param name="log">Log</param>
        public void RaiseOnCrashLog(FuzzerLog log)
        {
            if (log == null) return;
            _Logs.Add(log);
            OnCrashLog?.Invoke(this, log);
        }
        /// <summary>
        /// Raise end of test
        /// </summary>
        /// <param name="result">Result</param>
        void RaiseOnTestEnd(ETestResult result)
        {
            OnTestEnd?.Invoke(this, result);
        }
        /// <summary>
        /// Stop logic
        /// </summary>
        public bool Stop()
        {
            return true;
        }
        /// <summary>
        /// Play logic
        /// </summary>
        public bool Play()
        {
            return true;
        }
        /// <summary>
        /// Pause logic
        /// </summary>
        public bool Pause()
        {
            return true;
        }
    }
}