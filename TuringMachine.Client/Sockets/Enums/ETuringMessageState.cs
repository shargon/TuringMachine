namespace TuringMachine.Client.Sockets.Enums
{
    public enum ETuringMessageState : byte
    {
        ReadingHeader = 0,
        ReadingData = 1,
        Full = 2
    }
}