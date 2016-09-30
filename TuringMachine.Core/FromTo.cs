using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TuringMachine.Core.Design;

namespace TuringMachine.Core
{
    [JsonConverter(typeof(JsonFromToConverter))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FromTo<T> where T : IComparable
    {
        bool _AreSame;
        T _From, _To;

        /// <summary>
        /// Excludes
        /// </summary>
        
        [TypeConverter(typeof(ListArrayConverter))]
        public List<T> Excludes { get; set; }
        /// <summary>
        /// From
        /// </summary>
        [Category("Values")]
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
        [Category("Values")]
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
        [ReadOnly(true)]
        [Browsable(false)]
        public bool AreSame { get { return _AreSame; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Equal values</param>
        public FromTo(T values)
        {
            _From = _To = values;
            Excludes = new List<T>();
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
            Excludes = new List<T>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FromTo()
        {
            _To = _From = default(T);
            Excludes = new List<T>();
        }

        /// <summary>
        /// Return if are between from an To
        /// </summary>
        /// <param name="o">Object</param>
        public bool AreIn(T o)
        {
            return o.CompareTo(From) <= 0 && To.CompareTo(o) >= 0;
        }

        public override string ToString()
        {
            string ex = "";

            if (Excludes != null)
                ex = string.Join(",", Excludes);

            return
                (From.CompareTo(To) == 0 ? From.ToString() : From.ToString() + " - " + To.ToString())
                + (ex == "" ? "" : "![" + ex + "]");
        }
    }
}