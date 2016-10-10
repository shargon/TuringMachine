using System;
using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Responses
{
    public class OpenStreamMessageResponse : TuringMessage
    {
        /// <summary>
        /// Input Type
        /// </summary>
        public Guid Id { get; set; }

        public bool CanSeek { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanTimeout { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenStreamMessageResponse() : base(ETuringMessageType.OpenStreamResponse) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id</param>
        public OpenStreamMessageResponse(Guid id) : this()
        {
            Id = id;
        }
    }
}