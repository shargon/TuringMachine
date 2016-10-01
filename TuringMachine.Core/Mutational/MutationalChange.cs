using System.ComponentModel;
using TuringMachine.Core.Helpers;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Mutational
{
    public class MutationalChange
    {
        /// <summary>
        /// Weight for collision
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Byte to append: 0x41='A'
        /// </summary>
        [Category("Append")]
        public IGetValue<byte> AppendByte { get; set; }

        /// <summary>
        /// Remove x bytes: 1
        /// </summary>
        [Category("Remove")]
        public IGetValue<ushort> RemoveLength { get; set; }
        /// <summary>
        /// Append x bytes: 5000
        /// </summary>
        [Category("Append")]
        public IGetValue<ushort> AppendLength { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MutationalChange()
        {
            AppendByte = new FromToValue<byte>(byte.MinValue, byte.MaxValue);
            RemoveLength = new FromToValue<ushort>(1);
            AppendLength = new FromToValue<ushort>(1);
            Weight = 1;
            Description = "Unnamed";
        }
        /// <summary>
        /// Do the fuzz process
        /// </summary>
        /// <param name="realOffset">Real offset</param>
        public MutationLog Process(long realOffset)
        {
            // Removes
            ushort remove = RemoveLength.Get();

            // Appends
            ushort size = AppendLength.Get();

            if (size == 0)
                return remove > 0 ? new MutationLog(realOffset, remove) : null;

            byte[] data = new byte[size];
            RandomHelper.Randomize(data, 0, size, AppendByte);

            return new MutationLog(realOffset, data, remove);
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Weight.ToString() + " - " + Description;
        }
    }
}