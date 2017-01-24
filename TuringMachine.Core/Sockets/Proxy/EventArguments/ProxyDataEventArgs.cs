using System;
using TuringMachine.Core.Sockets.Proxy.Enums;

namespace TuringMachine.Core.Sockets.Proxy.EventArguments
{
    public class ProxyDataEventArgs : EventArgs
    {
        /// <summary>
        /// Bytes length
        /// </summary>
        public int Bytes;

        public ProxyDataEventArgs(int bytes)
        {
            Bytes = bytes;
        }
    }

    public class ProxyByteDataEventArgs : EventArgs
    {
        /// <summary>
        /// Data
        /// </summary>
        public byte[] Bytes;
        /// <summary>
        /// Source
        /// </summary>
        public ESource Source { get; private set; }

        public ProxyByteDataEventArgs(byte[] bytes, ESource source)
        {
            Bytes = bytes;
            Source = source;
        }
    }
}