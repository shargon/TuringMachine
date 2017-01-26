using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
{
    public class OpenStreamMessageRequest : TuringMessage
    {
        public bool UseMemoryStream { get; set; }
        //public bool CanRead { get; set; }
        //public bool CanSeek { get; set; }
        //public bool CanTimeout { get; set; }
        //public bool CanWrite { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenStreamMessageRequest() : base(ETuringMessageType.OpenStreamRequest) { }
    }
}