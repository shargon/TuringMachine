using System;
using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets.Messages.Requests
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