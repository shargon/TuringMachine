using System.Net;

namespace NRepeat
{
    public interface IProxy
    {
        IPEndPoint Server { get; set; }
        int Buffer { get; set; }
        bool Running { get; set; }
        void Start();
        void Stop();
    }
}