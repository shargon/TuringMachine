using System;
using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
{
    public class GetStreamLengthMessageRequest : TuringMessage
    {
        /// <summary>
        /// Input Type
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GetStreamLengthMessageRequest() : base(ETuringMessageType.GetStreamLengthRequest) { }
        /// <summary>
        /// Constructor
        /// </summary>
        public GetStreamLengthMessageRequest(Guid id) : this()
        {
            Id = id;
        }
    }
}