namespace TuringMachine.Core.Sockets.Enums
{
    public enum ETuringMessageState : byte
    {
        ReadingHeader = 0,
        ReadingData = 1,
    }
}