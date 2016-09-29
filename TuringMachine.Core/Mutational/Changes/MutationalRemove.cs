using System;
using TuringMachine.Core.Helpers;

namespace TuringMachine.Core.Mutational.Changes
{
    public class MutationalRemove : IMutationalChange
    {
        /// <summary>
        /// Bytes
        /// </summary>
        public FromTo<int> Count { get; set; }

        public MutationalRemove()
        {
            Count = new FromTo<int>(1);
        }
        /// <summary>
        /// Remove 'Count' bytes
        /// </summary>
        /// <param name="data">Data</param>
        public bool Process(ref byte[] data, ref int index, ref int length)
        {
            int size = RandomHelper.GetRandom(Count.From, Count.To);
            if (size <= 0) return false;

            /// Prevent negative buffer
            //size = Math.Min(length, size);

            length -= size;
            if (length <= 0)
            {
                data = new byte[] { };
                index = 0;
            }
            else
            {
                index += size;
            }

            return true;
        }
    }
}