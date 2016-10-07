using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using TuringMachine.Client.Sockets;
using TuringMachine.Client.Sockets.Enums;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Mutational;

namespace TuringMachine.Core
{
    /// <summary>
    /// [Client] <<----  Read Fuzz Information      <<----  [Server]  <<---- [Inputs]
    ///          ----->> Write Information for Fuz  ---------------------->>
    /// </summary>
    public class TuringServer
    {
        public delegate void delOnTestEnd(object sender, ETestResult result);
        public delegate void delOnCrashLog(object sender, FuzzerLog log);

        public event EventHandler OnListenChange;

        public event delOnTestEnd OnTestEnd;
        public event delOnCrashLog OnCrashLog;

        bool _Paused;
        EFuzzerState _State;
        TuringSocket _Socket;
        IPEndPoint _EndPoint = new IPEndPoint(IPAddress.Any, 7777);
        /// <summary>
        /// Inputs
        /// </summary>
        public ObservableCollection<FuzzerStat<IFuzzingInput>> Inputs { get; private set; }
        /// <summary>
        /// Configurations
        /// </summary>
        public ObservableCollection<FuzzerStat<IFuzzingConfig>> Configurations { get; private set; }
        /// <summary>
        /// Logs
        /// </summary>
        public ObservableCollection<FuzzerLog> Logs { get; private set; }
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
        /// State
        /// </summary>
        public EFuzzerState State { get { return _State; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public TuringServer()
        {
            Inputs = new ObservableCollection<FuzzerStat<IFuzzingInput>>();
            Configurations = new ObservableCollection<FuzzerStat<IFuzzingConfig>>();
            Logs = new ObservableCollection<FuzzerLog>();
        }
        /// <summary>
        /// Add input
        /// </summary>
        /// <param name="input">Input</param>
        public void AddInput(IFuzzingInput input)
        {
            if (Inputs == null) return;

            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
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

            Logs.Add(log);
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
            _State = EFuzzerState.Stopped;
            _Paused = false;
            if (_Socket != null)
            {
                _Socket.Dispose();
                _Socket = null;
            }
            return true;
        }
        /// <summary>
        /// Start logic
        /// </summary>
        public bool Start()
        {
            Stop();
            _State = EFuzzerState.Started;
            _Socket = TuringSocket.Bind(Listen);
            _Socket.OnMessage += _Socket_OnMessage;
            return true;
        }
        /// <summary>
        /// Pause logic
        /// </summary>
        public bool Pause()
        {
            _State = EFuzzerState.Paused;
            _Paused = !_Paused;
            return true;
        }
        /// <summary>
        /// Message logic
        /// </summary>
        /// <param name="sender">Socket</param>
        /// <param name="message">Message</param>
        void _Socket_OnMessage(TuringSocket sender, TuringMessage message)
        {
            if (_Paused)
            {
                // Send Paused signal
                return;
            }
            switch (message.Type)
            {
                case ETuringMessageType.ConfigMessage:
                    {
                        break;
                    }
            }
        }
    }
}