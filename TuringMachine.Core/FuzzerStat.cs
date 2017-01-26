using TuringMachine.Core.Enums;
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
        /// Type
        /// </summary>
        public string Type { get { return _Source == null ? null : _Source.Type; } }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get { return ToString(false); } }
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
        /// Increment
        /// </summary>
        /// <param name="result">Result</param>
        public void Increment(EFuzzingReturn result)
        {
            Tests++;

            switch (result)
            {
                case EFuzzingReturn.Crash: Crashes++; break;
                case EFuzzingReturn.Fail: Fails++; break;
            }
        }
        /// <summary>
        /// Reset stats
        /// </summary>
        public void Reset()
        {
            Tests = 0;
            Crashes = 0;
            Fails = 0;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() { return ToString(false); }
        /// <summary>
        /// String representation
        /// </summary>
        /// <param name="withData">With data</param>
        public string ToString(bool withData)
        {
            if (withData)
            {
                if (_Source == null) return "Tests: " + Tests.ToString() + " - Crashes: " + Crashes.ToString() + " - Fails: " + Fails.ToString();
                return _Source.ToString() + " {Tests: " + Tests.ToString() + " - Crashes: " + Crashes.ToString() + " - Fails: " + Fails.ToString() + "}";
            }

            if (_Source == null) return "";
            return _Source.ToString();
        }
    }
}