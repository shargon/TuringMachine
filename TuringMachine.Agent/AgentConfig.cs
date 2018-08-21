using System;
using System.IO;
using System.Net;
using System.Reflection;
using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Agent
{
    public class AgentConfig
    {
        int _NumTasks = 1, _RetrySeconds = 5;

        /// <summary>
        /// Server EndPoint
        /// </summary>
        public IPEndPoint TuringServer { get; set; }
        /// <summary>
        /// Tasks
        /// </summary>
        public int NumTasks
        {
            get { return _NumTasks; }
            set { _NumTasks = Math.Min(10, Math.Max(1, value)); }
        }
        /// <summary>
        /// RetrySeconds
        /// </summary>
        public int RetrySeconds
        {
            get { return _RetrySeconds; }
            set { _RetrySeconds = Math.Min(60, Math.Max(1, value)); }
        }
        /// <summary>
        /// Library dll
        /// </summary>
        public string AgentLibrary { get; set; }
        /// <summary>
        /// Agent ClassName
        /// </summary>
        public string AgentClassName { get; set; }
        /// <summary>
        /// Arguments
        /// </summary>
        public string AgentArguments { get; set; }
        /// <summary>
        /// Verbose
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Load from arguments
        /// </summary>
        public void LoadFromArguments(string[] args)
        {
            if (args == null || args.Length == 0) return;

            AgentLibrary = GetWord("AgentLibrary", args);
            AgentClassName = GetWord("AgentClassName", args);
            AgentArguments = GetWord("AgentArguments", args);

            string v = GetWord("RetrySeconds", args);
            if (!string.IsNullOrEmpty(v)) RetrySeconds = Convert.ToInt32(v);

            v = GetWord("NumTasks", args);
            if (!string.IsNullOrEmpty(v)) NumTasks = Convert.ToInt32(v);

            v = GetWord("TuringServer", args);
            if (!string.IsNullOrEmpty(v)) TuringServer = v.ToIpEndPoint();

            if(!string.IsNullOrEmpty(AgentArguments))
            {
                AgentArguments = Environment.ExpandEnvironmentVariables(AgentArguments);
                AgentArguments = File.ReadAllText(AgentArguments);
            }
        }
        string GetWord(string word, string[] args)
        {
            foreach (var s in args)
            {
                if (s.StartsWith(word + "=", StringComparison.OrdinalIgnoreCase))
                    return s.Substring(word.Length + 1);
            }
            return null;
        }
        /// <summary>
        /// Create agent
        /// </summary>
        public Type GetAgent()
        {
            if (string.IsNullOrEmpty(AgentLibrary)) return null;

            var asm = Assembly.LoadFrom(AgentLibrary);
            if (asm == null) return null;

            foreach (var t in ReflectionHelper.GetTypesAssignableFrom(typeof(ITuringMachineAgent), asm))
            {
                if (!ReflectionHelper.HavePublicConstructor(t)) continue;

                if (string.IsNullOrEmpty(AgentClassName) || t.Name == AgentClassName)
                    return t;
            }

            return null;
        }
    }
}