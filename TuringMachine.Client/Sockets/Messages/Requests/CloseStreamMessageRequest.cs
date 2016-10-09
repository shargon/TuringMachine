using System;
using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets.Messages.Requests
{
    public class CloseStreamMessageRequest : TuringMessage
    {
        /// <summary>
        /// Input Type
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CloseStreamMessageRequest() : base(ETuringMessageType.CloseStreamRequest) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id</param>
        public CloseStreamMessageRequest(Guid id) : this()
        {
            Id = id;
        }
    }
}