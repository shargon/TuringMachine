using System;
using System.IO;
using System.Net;
using TuringMachine.Core;
using TuringMachine.Core.Arguments;
using TuringMachine.Core.Detectors;
using TuringMachine.Core.Detectors.Windows;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets;
using TuringMachine.Core.Sockets.Proxy;
using TuringMachine.Core.Sockets.Proxy.Enums;

namespace TuringMachine.BasicAgents
{
    public class StartProcessAndInvisibleProxy : ITuringMachineAgent
    {
        const string ProxyVarName = "PROXY";

        public enum EFuzzingType
        {
            Server,
            Client,
        }

        /// <summary>
        /// Process
        /// </summary>
        public ProcessStartInfoEx[] Process { get; set; }
        /// <summary>
        /// Listen EndPoint
        /// </summary>
        public IPEndPoint ListenEndPoint { get; set; }
        /// <summary>
        /// Connect to
        /// </summary>
        public IPEndPoint ConnectTo { get; set; }
        /// <summary>
        /// Connect timeout
        /// </summary>
        public TimeSpan ConnectTimeout { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public EFuzzingType Type { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StartProcessAndInvisibleProxy()
        {
            ConnectTimeout = TimeSpan.FromSeconds(30);
            Type = EFuzzingType.Server;
        }

        /// <summary>
        /// Create proxy ( auto-dispose whith socket )
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="e">Arguments</param>
        public ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e)
        {
            var proxy = new TcpInvisibleProxy(ListenEndPoint, ConnectTo) { Tag = socket };

            proxy.OnCreateStream += Proxy_OnCreateStream;
            socket[ProxyVarName] = proxy;
            proxy.Start();

            // Create process
            return new WERDetector(Process);
        }

        Stream Proxy_OnCreateStream(object sender, Stream stream, ESource owner)
        {
            var proxy = (TcpInvisibleProxy)sender;
            var socket = (TuringSocket)proxy.Tag;

            switch (Type)
            {
                case EFuzzingType.Server: return owner == ESource.Server ? stream : new TuringStream(socket, stream);
                case EFuzzingType.Client: return owner == ESource.Client ? stream : new TuringStream(socket, stream);
            }

            return stream;
        }

        public void OnRun(TuringSocket socket, TuringAgentArgs e) { }

        public bool GetItsAlive(TuringSocket socket, TuringAgentArgs e)
        {
            var proxy = (TcpInvisibleProxy)socket[ProxyVarName];
            return proxy != null && proxy.Running && proxy.ConnectedClients > 0;
        }
    }
}