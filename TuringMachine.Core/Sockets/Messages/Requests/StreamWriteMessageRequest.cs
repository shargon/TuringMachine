using System;
using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
{
    public class StreamWriteMessageRequest : TuringMessage
    {
        /// <summary>
        /// Input Type
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Length
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamWriteMessageRequest() : base(ETuringMessageType.WriteStreamRequest) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id</param>
        public StreamWriteMessageRequest(Guid id) : this()
        {
            Id = id;
        }
    }
}