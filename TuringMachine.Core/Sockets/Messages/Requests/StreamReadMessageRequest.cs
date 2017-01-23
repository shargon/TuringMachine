using System;
using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
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
        /// Pre-Append data
        /// </summary>
        public byte[] PreAppend { get; set; }
        /// <summary>
        /// ReSeek when PreAppend
        /// </summary>
        public bool PreAppendReSeek { get; set; }

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