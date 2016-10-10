using System;
using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
{
    public class FlushStreamMessageRequest : TuringMessage
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FlushStreamMessageRequest() : base(ETuringMessageType.FlushStreamRequest) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id</param>
        public FlushStreamMessageRequest(Guid id) : this()
        {
            Id = id;
        }
    }
}