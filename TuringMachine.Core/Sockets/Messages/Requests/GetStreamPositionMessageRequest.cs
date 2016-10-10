using System;
using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
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