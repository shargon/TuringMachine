using TuringMachine.Core.Helpers;

namespace TuringMachine.Core.Mutational.Changes
{
    public class MutationalAppend : MutationalSwitch
    {
        /// <summary>
        /// Append Random bytes
        /// </summary>
        /// <param name="data">Data</param>
        public override byte[] Process(out int remove)
        {
            remove = 0;

            int size = RandomHelper.GetRandom(Count.From, Count.To, Count.Excludes);
            if (size <= 0) return null;

            byte[] ret = new byte[size];
            RandomHelper.Randomize(ret, 0, size, AppendByte.From, AppendByte.To, AppendByte.Excludes);

            return ret;
        }
    }
}