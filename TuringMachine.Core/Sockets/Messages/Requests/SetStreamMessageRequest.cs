using System;
using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages.Requests
{
    public class SetStreamMessageRequest : TuringMessage
    {
        public enum EMode : byte
        {
            Position = 0,
            Length = 1
        }

        /// <summary>
        /// Input Type
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public long Value { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public EMode ValueType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SetStreamMessageRequest() : base(ETuringMessageType.SetStreamRequest) { }
        /// <summary>
        /// Constructor
        /// </summary>
        public SetStreamMessageRequest(Guid id) : this()
        {
            Id = id;
        }
    }
}