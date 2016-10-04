using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets.Messages
{
    public class TuringConfigMessage : TuringMessage
    {
        public enum EInputType : byte
        {
            /// <summary>
            /// Get Random input
            /// </summary>
            Random = 0,
            /// <summary>
            /// Get Proxy input
            /// </summary>
            Proxy = 1
        }

        /// <summary>
        /// Input Type
        /// </summary>
        public EInputType InputType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data</param>
        public TuringConfigMessage(byte[] data) : base(ETuringMessageType.ConfigMessage, data)
        {
            InputType = (EInputType)data[0];
        }
        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns></returns>
        public override byte[] GetData()
        {
            return new byte[] { (byte)InputType };
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public TuringConfigMessage() : base(ETuringMessageType.ConfigMessage)
        {

        }
    }
}