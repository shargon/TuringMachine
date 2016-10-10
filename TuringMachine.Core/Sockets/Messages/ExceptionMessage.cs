using TuringMachine.Core.Sockets.Enums;

namespace TuringMachine.Core.Sockets.Messages
{
    public class ExceptionMessage : TuringMessage
    {
        /// <summary>
        /// Error
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionMessage() : base(ETuringMessageType.Exception) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="error">Error</param>
        public ExceptionMessage(string error) : this()
        {
            Error = error;
        }
    }
}