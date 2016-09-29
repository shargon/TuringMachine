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
        public override byte[] Process(out int remove)
        {
            remove = RandomHelper.GetRandom(Count.From, Count.To, Count.Excludes);
            return null;
        }
        public override string ToString()
        {
            return Count == null ? "NULL" : Count.ToString();
        }
    }
}