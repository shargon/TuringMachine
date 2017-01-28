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

        public StartProcessAndInvisibleProxy()
        {
            ConnectTimeout = TimeSpan.FromSeconds(30);
            Type = EFuzzingType.Server;
        }

        public override ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e)
        {
            // Create proxy ( auto-dispose whith socket )
            TcpInvisibleProxy proxy = new TcpInvisibleProxy(ListenEndPoint, ConnectTo) { Tag = socket };

            proxy.OnCreateStream += Proxy_OnCreateStream;
            socket[ProxyVarName] = proxy;
            proxy.Start();

            // Create process
            return new WERDetector(Process);
        }

        Stream Proxy_OnCreateStream(object sender, Stream stream, ESource owner)
        {
            TcpInvisibleProxy proxy = (TcpInvisibleProxy)sender;
            TuringSocket socket = (TuringSocket)proxy.Tag;

            switch (Type)
            {
                case EFuzzingType.Server: return owner == ESource.Server ? stream : new TuringStream(socket, stream);
                case EFuzzingType.Client: return owner == ESource.Client ? stream : new TuringStream(socket, stream);
            }

            return stream;
        }
        public override void OnRun(TuringSocket socket, TuringAgentArgs e) { }
        public override bool GetItsAlive(TuringSocket socket, TuringAgentArgs e)
        {
            TcpInvisibleProxy proxy = (TcpInvisibleProxy)socket[ProxyVarName];
            return proxy != null && proxy.Running && proxy.ConnectedClients > 0;
        }
    }
}