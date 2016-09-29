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
        public override byte[] Process(out int remove)
        {
            remove = RandomHelper.GetRandom(Count.From, Count.To, Count.Excludes);
            if (remove <= 0) return null;

            byte[] data = new byte[remove];
            RandomHelper.Randomize(data, 0, remove, AppendByte.From, AppendByte.To, AppendByte.Excludes);

            return data;
        }
        public override string ToString()
        {
            return
                "AppendByte: " + AppendByte == null ? "NULL" : AppendByte.ToString() + " / " +
                "Count: " + Count == null ? "NULL" : Count.ToString() + " / " +
                "Weight: " + Weight == null ? "NULL" : Weight.ToString();
        }
    }
}