using System;
using System.Net;
using System.Reflection;
using TuringMachine.Client;
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
            set
            {
                _NumTasks = Math.Max(1, value);
            }
        }
        /// <summary>
        /// RetrySeconds
        /// </summary>
        public int RetrySeconds
        {
            get { return _RetrySeconds; }
            set
            {
                _RetrySeconds = Math.Max(1, value);
            }
        }
        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel { get; set; }
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
        /// Load from arguments
        /// </summary>
        public void LoadFromArguments(string[] args)
        {
            if (args == null || args.Length == 0) return;

            throw new NotImplementedException();
        }
        /// <summary>
        /// Create agent
        /// </summary>
        public Type GetAgent()
        {
            if (string.IsNullOrEmpty(AgentLibrary)) return null;

            Assembly asm = Assembly.LoadFrom(AgentLibrary);
            if (asm == null) return null;

            foreach (Type t in ReflectionHelper.GetTypesAssignableFrom(typeof(ITuringMachineAgent), asm))
            {
                if (!ReflectionHelper.HavePublicConstructor(t)) continue;

                if (string.IsNullOrEmpty(AgentClassName) || t.Name == AgentClassName)
                    return t;
            }

            return null;
        }
    }
}