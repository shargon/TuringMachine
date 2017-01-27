using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Inputs
{
    public class EmptyFuzzingInput : IFuzzingInput
    {
        public string Type { get { return "Empty"; } }

        public byte[] GetStream() { return null; }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return "Empty Stream";
        }
    }
}