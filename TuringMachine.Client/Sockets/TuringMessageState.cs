using TuringMachine.Client.Sockets.Enums;

namespace TuringMachine.Client.Sockets
{
    class TuringMessageState
    {
        /// <summary>
        /// Source Socket
        /// </summary>
        public TuringSocket Source { get; private set; }
        /// <summary>
        /// Buffer
        /// </summary>
        public byte[] Buffer { get; private set; }
        /// <summary>
        /// Index
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// Return if its full message
        /// </summary>
        public ETuringMessageState State { get; private set; }
        /// <summary>
        /// MessageType
        /// </summary>
        public ETuringMessageType MessageType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source socket</param>
        public TuringMessageState(TuringSocket source)
        {
            Source = source;
            Clear();
        }
        /// <summary>
        /// Check Data
        /// </summary>
        public TuringMessage CheckData(int read)
        {
            Index += read;

            switch (State)
            {
                case ETuringMessageState.ReadingHeader:
                    {
                        if (Index == TuringMessage.HeaderLength)
                        {
                            int length;
                            ETuringMessageType type;
                            if (TuringMessage.ParseHeader(Buffer, 0, TuringMessage.HeaderLength, out length, out type))
                            {
                                Index = 0;
                                Buffer = new byte[length];
                                MessageType = type;
                                State = ETuringMessageState.ReadingData;

                                if (length == 0)
                                {
                                    Clear();
                                    return TuringMessage.Create(type, Buffer);
                                }
                            }
                        }
                        break;
                    }
                case ETuringMessageState.ReadingData:
                    {
                        if (Index == Buffer.Length)
                        {
                            TuringMessage ret = TuringMessage.Create(MessageType, Buffer);
                            Clear();
                            return ret;
                        }
                        break;
                    }
            }
            return null;
        }
        /// <summary>
        /// Clear state
        /// </summary>
        public void Clear()
        {
            Buffer = new byte[TuringMessage.HeaderLength];
            Index = 0;
            State = ETuringMessageState.ReadingHeader;
        }
    }
}