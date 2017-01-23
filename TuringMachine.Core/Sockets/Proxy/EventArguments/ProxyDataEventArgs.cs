using System;
using TuringMachine.Core.Sockets.Proxy.Enums;

namespace NRepeat
{
    public class ProxyDataEventArgs : EventArgs
    {
        public int Bytes;

        public ProxyDataEventArgs(int bytes)
        {
            Bytes = bytes;
        }
    }

    public class ProxyByteDataEventArgs : EventArgs
    {
        public byte[] Bytes;
        public ESource Source { get; private set; }

        public ProxyByteDataEventArgs(byte[] bytes, ESource source)
        {
            Bytes = bytes;
            Source = source;
        }
    }
}