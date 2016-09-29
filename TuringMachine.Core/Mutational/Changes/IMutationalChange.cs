namespace TuringMachine.Core.Mutational.Changes
{
    public class IMutationalChange
    {
        /// <summary>
        /// Weight for collision
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// Internal logic
        /// </summary>
        /// <param name="remove">Remove bytes</param>
        /// <returns>Return fuzz data</returns>
        public virtual byte[] Process(out int remove) { remove = 0; return null; }
        /// <summary>
        /// Constructor
        /// </summary>
        protected IMutationalChange() { }
        public override string ToString() { return Weight.ToString(); }
    }
}