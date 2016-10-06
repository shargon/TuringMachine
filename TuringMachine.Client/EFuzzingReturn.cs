namespace TuringMachine.Client
{
    public enum EFuzzingReturn : byte
    {
        /// <summary>
        /// Only test
        /// </summary>
        Test,
        /// <summary>
        /// Fail
        /// </summary>
        Fail,
        /// <summary>
        /// Crash
        /// </summary>
        Crash
    }
}