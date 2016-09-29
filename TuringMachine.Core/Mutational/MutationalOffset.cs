using System;
using TuringMachine.Core.Helpers;
using TuringMachine.Core.Mutational.Changes;

namespace TuringMachine.Core.Mutational
{
    public class MutationalOffset : Offset
    {
        float _FuzzPercent;
        int _Count, _MaxRandom;
        IMutationalChange[] _Changes;
        IMutationalChange[] _Steps;

        /// <summary>
        /// Changes
        /// </summary>
        public IMutationalChange[] Changes
        {
            get { return _Changes; }
            set { _Changes = value; Recall(); }
        }
        /// <summary>
        /// Fuzz Percent
        /// </summary>
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
        }
        /// <summary>
        /// Recall Changes
        /// </summary>
        void Recall()
        {
            _Count = 0;

            if (_Changes != null)
            {
                foreach (IMutationalChange c in _Changes)
                    _Count += c.Weight;

                _Steps = new IMutationalChange[_Count];

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
                _Steps = new IMutationalChange[] { };
                _MaxRandom = 0;
                _Count = 0;
            }
        }
        /// <summary>
        /// Get next mutation
        /// </summary>
        public IMutationalChange Get()
        {
            int r = RandomHelper.GetRandom(0, _MaxRandom, null);
            if (r < _Count)
                return _Steps[r];

            return null;
        }
    }
}