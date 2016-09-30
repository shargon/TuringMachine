using System.ComponentModel;
using TuringMachine.Core.Helpers;

namespace TuringMachine.Core.Mutational
{
    public class MutationalChange
    {
        /// <summary>
        /// Weight for collision
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Byte to append: 0x41='A'
        /// </summary>
        [Category("Append")]
        public FromTo<byte> AppendByte { get; set; }

        /// <summary>
        /// Remove x bytes: 1
        /// </summary>
        [Category("Remove")]
        public FromTo<ushort> RemoveLength { get; set; }
        /// <summary>
        /// Append x bytes: 5000
        /// </summary>
        [Category("Append")]
        public FromTo<ushort> AppendLength { get; set; }

        public MutationalChange()
        {
            AppendByte = new FromTo<byte>(byte.MinValue, byte.MaxValue);// { Excludes = new byte[] { 1, 2 } };
            RemoveLength = new FromTo<ushort>(1);
            AppendLength = new FromTo<ushort>(1);
            Weight = 1;
            Description = "Unnamed";
        }
        public MutationLog Process(long realOffset)
        {
            // Removes
            ushort remove = RandomHelper.GetRandom(RemoveLength.From, RemoveLength.To, RemoveLength.Excludes);

            // Appends
            ushort size = RandomHelper.GetRandom(AppendLength.From, AppendLength.To, AppendLength.Excludes);

            if (size == 0)
                return remove > 0 ? new MutationLog(realOffset, remove) : null;

            byte[] data = new byte[size];
            RandomHelper.Randomize(data, 0, size, AppendByte.From, AppendByte.To, AppendByte.Excludes);

            return new MutationLog(realOffset, data, remove);
        }
        public override string ToString()
        {
            return Weight.ToString() + " - " + Description;
        }
    }
}