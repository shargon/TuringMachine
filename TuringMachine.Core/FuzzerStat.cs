using System.ComponentModel;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core
{
    public class FuzzerStat<T> : INotifyPropertyChanged
        where T : IType
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int _Tests, _Crashes, _Fails;

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
        public int Tests
        {
            get { return _Tests; }
            set
            {
                if (_Tests == value) return;

                _Tests = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tests"));
            }
        }
        /// <summary>
        /// Crashes
        /// </summary>
        public int Crashes
        {
            get { return _Crashes; }
            set
            {
                if (_Crashes == value) return;

                _Crashes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Crashes"));
            }
        }
        /// <summary>
        /// Fails
        /// </summary>
        public int Fails
        {
            get { return _Fails; }
            set
            {
                if (_Fails == value) return;

                _Fails = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fails"));
            }
        }

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
            _Tests = 0;
            _Crashes = 0;
            _Fails = 0;
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