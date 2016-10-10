using System;
using System.Text;
using TuringMachine.Core.Sockets.Enums;
using TuringMachine.Core.Sockets.Messages;
using TuringMachine.Core.Sockets.Messages.Requests;
using TuringMachine.Core.Sockets.Messages.Responses;
using TuringMachine.Helpers;

namespace TuringMachine.Core.Sockets
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
        public TuringMessage(ETuringMessageType messageType)
        {
            Type = messageType;
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
                case ETuringMessageType.Exception: return SerializationHelper.DeserializeFromJson<ExceptionMessage>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.EndTask: return SerializationHelper.DeserializeFromJson<EndTaskMessage>(Encoding.UTF8.GetString(buffer));
                    
                case ETuringMessageType.OpenStreamRequest: return SerializationHelper.DeserializeFromJson<OpenStreamMessageRequest>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.CloseStreamRequest: return SerializationHelper.DeserializeFromJson<CloseStreamMessageRequest>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.GetStreamLengthRequest: return SerializationHelper.DeserializeFromJson<GetStreamLengthMessageRequest>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.GetStreamPositionRequest: return SerializationHelper.DeserializeFromJson<GetStreamPositionMessageRequest>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.SetStreamRequest: return SerializationHelper.DeserializeFromJson<SetStreamMessageRequest>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.FlushStreamRequest: return SerializationHelper.DeserializeFromJson<FlushStreamMessageRequest>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.ReadStreamRequest: return SerializationHelper.DeserializeFromJson<StreamReadMessageRequest>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.WriteStreamRequest: return SerializationHelper.DeserializeFromJson<StreamWriteMessageRequest>(Encoding.UTF8.GetString(buffer));

                case ETuringMessageType.BoolResponse: return SerializationHelper.DeserializeFromJson<BoolMessageResponse>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.LongResponse: return SerializationHelper.DeserializeFromJson<LongMessageResponse>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.OpenStreamResponse: return SerializationHelper.DeserializeFromJson<OpenStreamMessageResponse>(Encoding.UTF8.GetString(buffer));
                case ETuringMessageType.ByteArrayResponse: return SerializationHelper.DeserializeFromJson<ByteArrayMessageResponse>(Encoding.UTF8.GetString(buffer));
            }

            return new TuringMessage(messageType);
        }
        /// <summary>
        /// Get Data
        /// </summary>
        public virtual byte[] GetData()
        {
            return Encoding.UTF8.GetBytes(SerializationHelper.SerializeToJson(this, false));
        }
    }
}