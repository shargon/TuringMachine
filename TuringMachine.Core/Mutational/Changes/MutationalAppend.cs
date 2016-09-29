using TuringMachine.Core.Helpers;
using System.Linq;

namespace TuringMachine.Core.Mutational.Changes
{
    public class MutationalAppend : MutationalSwitch
    {
        public enum EMode { AtBegin, AtEnd }
        /// <summary>
        /// Mode
        /// </summary>
        public EMode Mode { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MutationalAppend() : base()
        {
            Mode = EMode.AtBegin;
        }
        /// <summary>
        /// Append Random bytes
        /// </summary>
        /// <param name="data">Data</param>
        public override bool Process(ref byte[] data, ref int index, ref int length)
        {
            int size = RandomHelper.GetRandom(Count.From, Count.To);
            if (size <= 0) return false;

            byte[] ret = new byte[size];
            RandomHelper.Randomize(ret, 0, size, AppendByte.From, AppendByte.To);

            if (Mode == EMode.AtBegin) data = ret.Concat(data.Skip(index).Take(length)).ToArray();
            else data = data.Skip(index).Take(length).Concat(ret).ToArray();

            index = 0;
            length = data.Length;
            return true;
        }
    }
}