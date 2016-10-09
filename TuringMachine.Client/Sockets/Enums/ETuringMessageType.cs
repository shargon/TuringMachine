namespace TuringMachine.Client.Sockets.Enums
{
    public enum ETuringMessageType : byte
    {
        None = 0,
        Exception = 1,
        EndTask = 2,

        OpenStreamRequest = 10,
        CloseStreamRequest = 11,
        GetStreamLengthRequest = 12,
        GetStreamPositionRequest = 13,
        SetStreamRequest = 14,
        FlushStreamRequest = 15,
        ReadStreamRequest = 16,
        WriteStreamRequest = 17,

        OpenStreamResponse = 100,
        BoolResponse = 101,
        LongResponse = 102,
        ByteArrayResponse = 103,
    }
}