using System;
using System.Diagnostics;
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
    public class StartProcessAndProxy : ITuringMachineAgent
    {
        const string ProxyVarName = "PROXY";
        public enum EFuzzingType
        {
            ClientToServer,
            ServerToClient,
        }

        /// <summary>
        /// Process path
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Arguments
        /// </summary>
        public string Arguments { get; set; }
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

        public StartProcessAndProxy()
        {
            ConnectTimeout = TimeSpan.FromSeconds(30);
            Type = EFuzzingType.ClientToServer;
        }

        public override ICrashDetector GetCrashDetector(TuringSocket socket, TuringAgentArgs e)
        {
            // Create proxy ( auto-dispose whith socket )
            TcpInvisibleProxy proxy = new TcpInvisibleProxy(ListenEndPoint, ConnectTo) { Tag = socket };

            //proxy.OnCreateStream += Proxy_OnCreateStream;
            socket[ProxyVarName] = proxy;
            proxy.Start();

            // Create process
            return new WERDetector(new ProcessStartInfo(FileName, Arguments)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            });
        }

        Stream Proxy_OnCreateStream(object sender, Stream stream, ESource owner)
        {
            TcpInvisibleProxy proxy = (TcpInvisibleProxy)sender;
            TuringSocket socket = (TuringSocket)proxy.Tag;

            switch (Type)
            {
                case EFuzzingType.ClientToServer: return owner == ESource.Client ? stream : new TuringStream(socket, stream);
                case EFuzzingType.ServerToClient: return owner == ESource.Server ? stream : new TuringStream(socket, stream);
            }

            return stream;
        }
        public override void OnRun(TuringSocket socket, TuringAgentArgs e) { }
        public override bool GetItsAlive(TuringSocket socket, TuringAgentArgs e)
        {
            TcpInvisibleProxy proxy = (TcpInvisibleProxy)socket[ProxyVarName];
            return proxy != null && proxy.Running;
        }
    }
}