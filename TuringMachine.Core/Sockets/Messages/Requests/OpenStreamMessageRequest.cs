using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
{
    public class OpenStreamMessageRequest : TuringMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenStreamMessageRequest() : base(ETuringMessageType.OpenStreamRequest) { }
    }
}