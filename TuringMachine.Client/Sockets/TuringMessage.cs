using System;
using TuringMachine.Client.Sockets.Enums;
using TuringMachine.Client.Sockets.Messages;
using TuringMachine.Helpers;

namespace TuringMachine.Client.Sockets
{
    public class TuringMessage
    {
        public const byte HeaderLength = 5;

        /// <summary>
        /// Type
        /// </summary>
        public ETuringMessageType Type { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageType">Message Type</param>
        /// <param name="data">Data</param>
        public TuringMessage(ETuringMessageType messageType, byte[] data = null)
        {
            Type = messageType;
        }
        /// <summary>
        /// Get Data
        /// </summary>
        public virtual byte[] GetData()
        {
            return null;
        }
        /// <summary>
        /// GetHeader of message
        /// </summary>
        /// <param name="dataLength">Data length</param>
        public byte[] GetHeader(int dataLength)
        {
            byte[] data = BitHelper.GetBytes(dataLength);

            Array.Resize(ref data, HeaderLength);

            data[HeaderLength - 1] = (byte)Type;
            return data;
        }
        /// <summary>
        /// Parse header message
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="index">Index</param>
        /// <param name="count">Count</param>
        /// <param name="length">Length</param>
        /// <param name="messageType">Type</param>
        /// <returns></returns>
        public static bool ParseHeader(byte[] buffer, int index, byte count, out int length, out ETuringMessageType messageType)
        {
            if (count != HeaderLength)
            {
                length = 0;
                messageType = ETuringMessageType.None;
                return false;
            }

            length = BitHelper.ToInt32(buffer, index);
            messageType = (ETuringMessageType)buffer[HeaderLength - 1];
            return true;
        }
        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="messageType">Type</param>
        /// <param name="buffer">Buffer</param>
        public static TuringMessage Create(ETuringMessageType messageType, byte[] buffer)
        {
            switch (messageType)
            {
                case ETuringMessageType.ConfigMessage: return new TuringConfigMessage(buffer);
            }

            return new TuringMessage(messageType, buffer);
        }
    }
}