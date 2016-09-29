namespace TuringMachine.Core.Mutational.Changes
{
    public interface IMutationalChange
    {
        /// <summary>
        /// Internal logic
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        /// <returns>Return True if fuzzed something</returns>
        bool Process(ref byte[] data, ref int index, ref int length);
    }
}