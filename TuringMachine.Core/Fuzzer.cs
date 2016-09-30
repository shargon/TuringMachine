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
    public class Fuzzer
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

        public Fuzzer()
        {
            Inputs = new List<FuzzerStat<IFuzzingInput>>();
            Configurations = new List<FuzzerStat<IFuzzingConfig>>();
        }

        public void AddFileInput(string file)
        {
            if (!File.Exists(file)) return;

            FileFuzzingInput input = new FileFuzzingInput(file);
            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
            OnInputsChange?.Invoke(this, EventArgs.Empty);
        }
        public void AddTcpInput(IPEndPoint endPoint)
        {
            if (endPoint == null) return;

            TcpFuzzingInput input = new TcpFuzzingInput(endPoint);
            Inputs.Add(new FuzzerStat<IFuzzingInput>(input));
            OnInputsChange?.Invoke(this, EventArgs.Empty);
        }
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

        public void AddTcpInput(object endPoint)
        {
            throw new NotImplementedException();
        }
    }
}