using TuringMachine.Core.Helpers;

namespace TuringMachine.Core.Mutational.Changes
{
    public class MutationalSwitch : IMutationalChange
    {
        /// <summary>
        /// Byte
        /// </summary>
        public FromTo<byte> AppendByte { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public FromTo<int> Count { get; set; }

        public MutationalSwitch()
        {
            AppendByte = new FromTo<byte>(byte.MinValue, byte.MaxValue);
            Count = new FromTo<int>(1);
        }
        public virtual bool Process(ref byte[] data, ref int index, ref int length)
        {
            int size = RandomHelper.GetRandom(Count.From, Count.To);
            if (size <= 0) return false;

            if (size == length)
            {
                // Same size
                RandomHelper.Randomize(data, index, size, AppendByte.From, AppendByte.To);
            }
            else
            {
                int s = size - length;
                if (s > 0)
                {
                    // More size
                    data = new byte[size];

                    index = 0;
                    length = size;

                    RandomHelper.Randomize(data, 0, length, AppendByte.From, AppendByte.To);
                }
                else
                {
                    // Less size
                    RandomHelper.Randomize(data, index, size, AppendByte.From, AppendByte.To);
                }
            }

            return true;
        }
    }
}