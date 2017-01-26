using System.Net;

namespace TuringMachine.Core.Sockets.Proxy.Interfaces
{
    public interface IProxy
    {
        IPEndPoint Server { get; set; }
        int Buffer { get; set; }
        bool Running { get; }
        void Start();
        void Stop();
    }
}