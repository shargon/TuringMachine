using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Core.Helpers;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core
{
    [JsonConverter(typeof(JsonFromToConverter))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FixedValue<T> : IGetValue<T> where T : struct, IComparable
    {
        /// <summary>
        /// From
        /// </summary>
        [Category("Values")]
        [TypeConverter(typeof(ListArrayConverter))]
        public List<T> Allowed { get; set; }
        /// <summary>
        /// Class Name
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public string Name { get { return "Fixed"; } }
        /// <summary>
        /// Constructor
        /// </summary>
        public FixedValue()
        {
            Allowed = new List<T>();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Start values</param>
        public FixedValue(params T[] values) : this()
        {
            if (values != null) Allowed.AddRange(values);
        }
        /// <summary>
        /// Return if are between from an To
        /// </summary>
        /// <param name="o">Object</param>
        public bool ItsValid(T o)
        {
            return Allowed.Contains(o);
        }
        /// <summary>
        /// Get next item
        /// </summary>
        public T Get()
        {
            if (Allowed.Count <= 0) return default(T);

            int r = RandomHelper.GetRandom(0, Allowed.Count - 1);
            return Allowed[r];
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            if (Allowed == null || Allowed.Count <= 0) return "Empty";
            return string.Join(",", Allowed);
        }
    }
}