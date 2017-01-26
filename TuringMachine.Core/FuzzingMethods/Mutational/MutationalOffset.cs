using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Helpers;
using TuringMachine.Core.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TuringMachine.Core.Enums;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

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
        /// Fuzz Percent Type
        /// </summary>
        [Category("1 - Condition")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EFuzzingPercentType FuzzPercentType { get; set; }
        /// <summary>
        /// MaxChanges
        /// </summary>
        [Category("1 - Condition")]
        public IGetValue<ushort> MaxChanges { get; set; }

        class step
        {
            public int MaxChanges = 0;
            public List<ulong> FuzzIndex = new List<ulong>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MutationalOffset()
        {
            FuzzPercent = new FromToValue<double>(0, 5);
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

                int x = 0;
                foreach (MutationalChange c in _Changes)
                {
                    if (!c.Enabled) continue;

                    for (int w = 0; w < c.Weight; w++, x++)
                        _Steps[x] = c;
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
            step s = new step();
            // Max changes
            s.MaxChanges = MaxChanges.Get();

            if (FuzzPercentType == EFuzzingPercentType.PeerStream)
            {
                // Fill indexes
                long length = stream.Length;
                for (long x = Math.Max(1, (long)((length * FuzzPercent.Get()) / 100.0)); x >= 0; x--)
                {
                    ulong value;

                    do
                    {
                        value = Math.Min((ulong)length, ValidOffset.Get());
                    }
                    while (!s.FuzzIndex.Contains(value));

                    s.FuzzIndex.Add(value);
                }
            }

            stream.Variables["Config_" + index.ToString()] = s;
        }
        /// <summary>
        /// Get next mutation change (if happend)
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="index">Index</param>
        public MutationalChange Get(FuzzingStream stream, ulong index)
        {
            // Check Max changes
            step s = (step)stream.Variables["Config_" + index.ToString()];
            if (stream.Log.Length >= s.MaxChanges) return null;

            switch (FuzzPercentType)
            {
                case EFuzzingPercentType.PeerByte:
                    {
                        // Check Percent
                        double value = FuzzPercent.Get();

                        if (!RandomHelper.IsRandomPercentOk(value)) return null;

                        // Get Item
                        return RandomHelper.GetRandom(_Steps);
                    }
                case EFuzzingPercentType.PeerStream:
                    {
                        if (!s.FuzzIndex.Contains(index)) return null;

                        return RandomHelper.GetRandom(_Steps);
                    }
            }

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