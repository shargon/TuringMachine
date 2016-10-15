using System;
using System.Collections.Generic;

namespace TuringMachine.Core.Collections
{
    public class VariableCollection<TKey, TValue>
    {
        Dictionary<TKey, TValue> _Internal;

        /// <summary>
        /// Values
        /// </summary>
        public IEnumerable<TValue> Values { get { return _Internal.Values; } }
        /// <summary>
        /// Keys
        /// </summary>
        public IEnumerable<TKey> Key { get { return _Internal.Keys; } }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="key">Key</param>
        public TValue this[TKey key]
        {
            get
            {
                TValue ret;
                if (_Internal.TryGetValue(key, out ret)) return ret;
                return default(TValue);
            }
            set
            {
                if (key == null) return;

                if (value == null) _Internal.Remove(key);
                else _Internal[key] = value;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public VariableCollection()
        {
            _Internal = new Dictionary<TKey, TValue>();
        }
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            foreach (object o in _Internal.Values)
                if (o is IDisposable) ((IDisposable)o).Dispose();

            _Internal.Clear();
        }
    }
}