using System.ComponentModel;
using TuringMachine.Helpers;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.FuzzingMethods.Patchs;
using Newtonsoft.Json;

namespace TuringMachine.Core.FuzzingMethods.Mutational
{
    public class MutationalChange
    {
        bool _RemoveAndAppendAreSame;
        IGetValue<ushort> _RemoveLength, _AppendLength;

        /// <summary>
        /// Weight for collision
        /// </summary>
        [Description("Set the weight")]
        [Category("3 - Select")]
        public int Weight { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        [Description("Set Enabled")]
        [Category("3 - Select")]
        public bool Enabled { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [Description("Set the description")]
        [Category("4 - Info")]
        public string Description { get; set; }
        /// <summary>
        /// Byte to append: 0x41='A'
        /// </summary>
        [Category("1 - Append")]
        [Description("Set the kind of bytes for add")]
        public IGetValue<byte> AppendByte { get; set; }

        /// <summary>
        /// Remove x bytes: 1
        /// </summary>
        [Category("2 - Remove")]
        [Description("Set the remove length value")]
        public IGetValue<ushort> RemoveLength { get { return _RemoveLength; } set { _RemoveLength = value; Recall(); } }
        /// <summary>
        /// Append x bytes: 5000
        /// </summary>
        [Category("1 - Append")]
        [Description("Set the append length value")]
        public IGetValue<ushort> AppendLength { get { return _AppendLength; } set { _AppendLength = value; Recall(); } }
        /// <summary>
        /// Remove and append are same
        /// </summary>
        [JsonIgnore]
        [Description("Get if are used the same random value for Remove&Append")]
        [Category("4 - Info")]
        public bool RemoveAndAppendAreSame { get { return _RemoveAndAppendAreSame; } }

        /// <summary>
        /// Check if remove and Append are same
        /// </summary>
        void Recall()
        {
            if (_RemoveLength != null && _AppendLength != null)
                _RemoveAndAppendAreSame = _RemoveLength.GetType() == _AppendLength.GetType() && _AppendLength.ToString() == _RemoveLength.ToString();
            else
                _RemoveAndAppendAreSame = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MutationalChange()
        {
            AppendByte = new FromToValue<byte>(byte.MinValue, byte.MaxValue);
            Weight = 1;
            Description = "Unnamed";

            _RemoveLength = new FromToValue<ushort>(1);
            _AppendLength = new FromToValue<ushort>(1);
            _RemoveAndAppendAreSame = true;
            Enabled = true;
        }
        /// <summary>
        /// Do the fuzz process
        /// </summary>
        /// <param name="realOffset">Real offset</param>
        public PatchChange Process(long realOffset)
        {
            // Removes
            ushort remove = RemoveLength.Get();

            // Appends
            ushort size = _RemoveAndAppendAreSame ? remove : AppendLength.Get();

            if (size == 0)
                return remove > 0 ? new PatchChange(Description, realOffset, remove) : null;

            byte[] data = new byte[size];
            RandomHelper.Randomize(data, 0, size, AppendByte);

            return new PatchChange(Description, realOffset, data, remove);
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Weight.ToString() + " - " + Description;
        }
    }
}