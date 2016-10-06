using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Core
{
    [JsonConverter(typeof(JsonFromToConverter))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FromToValue<T> : IGetValue<T> where T : struct, IComparable
    {
        bool _AreSame;
        T _From, _To;

        Type _Type;

        static Type TypeByte = typeof(byte);
        static Type TypeUInt16 = typeof(ushort);
        static Type TypeInt32 = typeof(int);
        static Type TypeInt64 = typeof(long);

        /// <summary>
        /// Class name
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public string Name { get { return "From-To"; } }
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
        public FromToValue(T values) : this(values, values) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="to">To value</param>
        public FromToValue(T from, T to) : this()
        {
            _From = from;
            _To = to;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public FromToValue()
        {
            _To = _From = default(T);
            Excludes = new List<T>();
            _Type = typeof(T);
        }
        /// <summary>
        /// Return if are between from an To
        /// </summary>
        /// <param name="o">Object</param>
        public bool ItsValid(T o)
        {
            if (From.CompareTo(o) <= 0 && To.CompareTo(o) >= 0)
            {
                if (Excludes.Contains(o)) return false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get next item
        /// </summary>
        public T Get()
        {
            if (_AreSame) return From;

            T ret = default(T);

            if (_Type == TypeByte)
            {
                // Get random byte
                do { ret = (T)Convert.ChangeType(RandomHelper.GetRandom(Convert.ToByte(From), Convert.ToByte(To)), _Type); }
                while (Excludes.Contains(ret));
            }
            else
            {
                if (_Type == TypeUInt16)
                {
                    // Get ushort byte
                    do { ret = (T)Convert.ChangeType(RandomHelper.GetRandom(Convert.ToUInt16(From), Convert.ToUInt16(To)), _Type); }
                    while (Excludes.Contains(ret));
                }
                else
                {
                    if (_Type == TypeInt32)
                    {
                        // Get int byte
                        do { ret = (T)Convert.ChangeType(RandomHelper.GetRandom(Convert.ToInt32(From), Convert.ToInt32(To)), _Type); }
                        while (Excludes.Contains(ret));
                    }
                    else
                    {
                        if (_Type == TypeInt64)
                        {
                            // Get int byte
                            do { ret = (T)Convert.ChangeType(RandomHelper.GetRandom(Convert.ToInt64(From), Convert.ToInt64(To)), _Type); }
                            while (Excludes.Contains(ret));
                        }
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// String representation
        /// </summary>
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