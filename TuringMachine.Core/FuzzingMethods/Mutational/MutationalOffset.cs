using System;
using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Helpers;
using TuringMachine.Core.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TuringMachine.Core.FuzzingMethods.Mutational
{
    public class MutationalOffset
    {
        float _FuzzPercent;
        ObservableCollection<MutationalChange> _Changes;
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
        public ObservableCollection<MutationalChange> Changes
        {
            get { return _Changes; }
            set
            {
                if (value == _Changes) return;

                if (_Changes != null) _Changes.CollectionChanged -= _Changes_CollectionChanged;
                if (value != null) value.CollectionChanged += _Changes_CollectionChanged;

                _Changes = value;
                Recall();
            }
        }

        void _Changes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) { Recall(); }

        /// <summary>
        /// Fuzz Percent
        /// </summary>
        [Category("Condition")]
        public float FuzzPercent { get { return _FuzzPercent; } set { _FuzzPercent = Math.Max(0, Math.Min(value, 100)); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public MutationalOffset()
        {
            FuzzPercent = 5F;
            ValidOffset = new FromToValue<ulong>(ulong.MinValue, ulong.MaxValue);
            Changes = new ObservableCollection<MutationalChange>();
            Description = "Unnamed";
        }
        /// <summary>
        /// Recall Changes
        /// </summary>
        void Recall()
        {
            if (_Changes != null)
            {
                int count = 0;
                foreach (MutationalChange c in _Changes)
                    count += c.Weight;

                _Steps = new MutationalChange[count];

                int w = 0;
                for (int x = 0, y = 0; x < count; x++)
                {
                    if (_Changes[y].Weight <= w)
                    {
                        y++;
                        w = 0;
                    }
                    else w++;

                    _Steps[x] = _Changes[y];
                }
            }
            else
            {
                _Steps = new MutationalChange[] { };
            }
        }
        /// <summary>
        /// Get next mutation change (if happend)
        /// </summary>
        public MutationalChange Get()
        {
            // Check Percent
            if (!RandomHelper.IsRandomPercentOk(_FuzzPercent)) return null;

            // Get Item
            return RandomHelper.GetRandom(_Changes);
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