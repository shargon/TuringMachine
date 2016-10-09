using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets.Messages.Requests
{
    public class OpenStreamMessageRequest : TuringMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenStreamMessageRequest() : base(ETuringMessageType.OpenStreamRequest) { }
    }
}