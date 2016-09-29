using System;

namespace TuringMachine.Core
{
    public class FromTo<T> where T : IComparable
    {
        bool _AreSame;
        T _From, _To;
        private int v;

        /// <summary>
        /// From
        /// </summary>
        public T From
        {
            get { return _From; }
            set
            {
                _From = value;
                _AreSame = From.CompareTo(To) == 0;
            }
        }
        /// <summary>
        /// To
        /// </summary>
        public T To
        {
            get { return _To; }
            set
            {
                _To = value;
                _AreSame = From.CompareTo(To) == 0;
            }
        }

        /// <summary>
        /// Return if are the same
        /// </summary>
        public bool AreSame { get { return _AreSame; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Equal values</param>
        public FromTo(T values)
        {
            _From = values;
            _To = values;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="to">To value</param>
        public FromTo(T from, T to)
        {
            _From = from;
            _To = to;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FromTo() { }

        /// <summary>
        /// Return if are between from an To
        /// </summary>
        /// <param name="o">Object</param>
        public bool AreBetween(T o)
        {
            return o.CompareTo(From) <= 0 && To.CompareTo(o) >= 0;
        }
    }
}