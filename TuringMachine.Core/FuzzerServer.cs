using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
        public event EventHandler OnInputsChange;
        public event EventHandler OnConfigurationsChange;
        public event EventHandler OnListenChange;

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
        }
        /// <summary>
        /// Add File input
        /// </summary>
        /// <param name="file">File</param>
        public void AddFileInput(string file)
        {
            if (!File.Exists(file)) return;

            FileFuzzingInput input = new FileFuzzingInput(file);
            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
            OnInputsChange?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Add tcp query input
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        public void AddTcpQueryInput(IPEndPoint endPoint)
        {
            if (endPoint == null) return;

            TcpQueryFuzzingInput input = new TcpQueryFuzzingInput(endPoint);
            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
            OnInputsChange?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Add tcp proxy input
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        public void AddTcpProxyInput(IPEndPoint endPoint)
        {
            if (endPoint == null) return;

            TcpProxyFuzzingInput input = new TcpProxyFuzzingInput(endPoint);
            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
            OnInputsChange?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Add execution input
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="args">Args</param>
        public void AddExecuteInput(string path, string args)
        {
            if (string.IsNullOrEmpty(path)) return;

            ExecutionFuzzingInput input = new ExecutionFuzzingInput(path,args);
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
    }
}