using System;
using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets.Messages.Requests
{
    public class StreamReadMessageRequest : TuringMessage
    {
        /// <summary>
        /// Input Type
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Length
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamReadMessageRequest() : base(ETuringMessageType.ReadStreamRequest) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id</param>
        public StreamReadMessageRequest(Guid id) : this()
        {
            Id = id;
        }
    }
}