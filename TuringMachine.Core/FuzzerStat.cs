using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core
{
    public class FuzzerStat<T> where T : IType
    {
        T _Source;
        /// <summary>
        /// Source
        /// </summary>
        public T Source { get { return _Source; } }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get { return ToString(); } }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return _Source.Type; } }
        /// <summary>
        /// Count
        /// </summary>
        public int Tests { get; set; }
        /// <summary>
        /// Crashes
        /// </summary>
        public int Crashes { get; set; }
        /// <summary>
        /// Fails
        /// </summary>
        public int Fails { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source</param>
        public FuzzerStat(T source)
        {
            _Source = source;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return _Source.ToString();
        }
    }
}