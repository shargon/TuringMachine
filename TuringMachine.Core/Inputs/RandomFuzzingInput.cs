using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Core.Inputs
{
    public class RandomFuzzingInput : IFuzzingInput
    {
        /// <summary>
        /// Length
        /// </summary>
        public FromToValue<long> Length { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Random"; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filename">File</param>
        public RandomFuzzingInput(FromToValue<long> length)
        {
            Length = length;
        }
        /// <summary>
        /// Get file stream
        /// </summary>
        public byte[] GetStream()
        {
            byte[] l = new byte[(int)Length.Get()];

            for (int x = l.Length - 1; x >= 0; x--)
                l[x] = RandomHelper.GetRandom((byte)0, (byte)255);

            return l;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return "Random " + Length.ToString() + " bytes";
        }
    }
}