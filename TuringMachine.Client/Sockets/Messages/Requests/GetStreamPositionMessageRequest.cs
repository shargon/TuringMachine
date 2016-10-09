using System;
using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets.Messages.Requests
{
    public class GetStreamPositionMessageRequest : TuringMessage
    {
        /// <summary>
        /// Input Type
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GetStreamPositionMessageRequest() : base(ETuringMessageType.GetStreamPositionRequest) { }
        /// <summary>
        /// Constructor
        /// </summary>
        public GetStreamPositionMessageRequest(Guid id) : this()
        {
            Id = id;
        }
    }
}