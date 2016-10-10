namespace TuringMachine.Core.Enums
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