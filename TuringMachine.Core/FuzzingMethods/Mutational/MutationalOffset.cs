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
        ObservableCollection<MutationalChange> _Changes;
        MutationalChange[] _Steps;

        /// <summary>
        /// Description
        /// </summary>
        [Category("3 - Info")]
        public string Description { get; set; }
        /// <summary>
        /// Valid offset
        /// </summary>
        [Category("1 - Condition")]
        public IGetValue<ulong> ValidOffset { get; set; }
        /// <summary>
        /// Changes
        /// </summary>
        [TypeConverter(typeof(ListArrayReadOnlyConverter))]
        [Category("2 - Collection")]
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
        [Category("1 - Condition")]
        public IGetValue<double> FuzzPercent { get; set; }
        /// <summary>
        /// MaxChanges
        /// </summary>
        [Category("1 - Condition")]
        public IGetValue<ushort> MaxChanges { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MutationalOffset()
        {
            FuzzPercent = new FromToValue<double>(0F, 5F);
            MaxChanges = new FromToValue<ushort>(0, 2);
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
                {
                    if (!c.Enabled) continue;
                    count += c.Weight;
                }

                _Steps = new MutationalChange[count];

                int w = 0;
                for (int x = 0, y = 0; x < count; x++)
                {
                    MutationalChange c = _Changes[y];
                    if (!c.Enabled) continue;

                    if (c.Weight <= w)
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
        /// Init for
        /// </summary>
        /// <param name="stream">Stream</param>
        public void InitFor(FuzzingStream stream, int index)
        {
            // Max changes
            stream.Variables["MaxChanges_" + index.ToString()] = MaxChanges.Get();
        }
        /// <summary>
        /// Get next mutation change (if happend)
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="index">Index</param>
        public MutationalChange Get(FuzzingStream stream, int index)
        {
            // Check Max changes
            ushort max = (ushort)stream.Variables["MaxChanges_" + index.ToString()];
            if (stream.Log.Length >= max) return null;

            // Check Percent
            double value = FuzzPercent.Get();

            if (!RandomHelper.IsRandomPercentOk(value)) return null;

            // Get Item
            return RandomHelper.GetRandom(_Steps);
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