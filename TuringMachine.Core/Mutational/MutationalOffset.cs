using System;
using System.Collections.Generic;
using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Core.Helpers;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Mutational
{
    public class MutationalOffset
    {
        float _FuzzPercent;
        int _Count, _MaxRandom;
        List<MutationalChange> _Changes;
        MutationalChange[] _Steps;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Valid offset
        /// </summary>
        [Category("Condition")]
        public IGetValue<ulong> ValidOffset { get; set; }
        /// <summary>
        /// Changes
        /// </summary>
        [TypeConverter(typeof(ListArrayReadOnlyConverter))]
        public List<MutationalChange> Changes
        {
            get { return _Changes; }
            set { _Changes = value; Recall(); }
        }
        /// <summary>
        /// Fuzz Percent
        /// </summary>
        [Category("Condition")]
        public float FuzzPercent
        {
            get { return _FuzzPercent; }
            set
            {
                _FuzzPercent = Math.Max(0, Math.Min(value, 100));
                Recall();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MutationalOffset()
        {
            FuzzPercent = 5F;
            _Count = 0;
            _MaxRandom = 0;
            ValidOffset = new FromToValue<ulong>(ulong.MinValue, ulong.MaxValue);
            Changes = new List<MutationalChange>();
            Description = "Unnamed";
        }
        /// <summary>
        /// Recall Changes
        /// </summary>
        void Recall()
        {
            _Count = 0;

            if (_Changes != null)
            {
                foreach (MutationalChange c in _Changes)
                    _Count += c.Weight;

                _Steps = new MutationalChange[_Count];

                int w = 0;
                for (int x = 0, y = 0; x < _Count; x++)
                {
                    if (_Changes[y].Weight <= w)
                    {
                        y++;
                        w = 0;
                    }
                    else w++;

                    _Steps[x] = _Changes[y];
                }

                _MaxRandom = (int)((_Count * 100) / _FuzzPercent);
            }
            else
            {
                _Steps = new MutationalChange[] { };
                _MaxRandom = 0;
                _Count = 0;
            }
        }
        /// <summary>
        /// Get next mutation change (if happend)
        /// </summary>
        public MutationalChange Get()
        {
            int r = RandomHelper.GetRandom(0, _MaxRandom);
            if (r < _Count)
                return _Steps[r];

            return null;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Description;
        }
    }
}