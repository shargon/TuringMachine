namespace TuringMachine.Core
{
    public class Offset
    {
        /// <summary>
        /// Offset
        /// </summary>
        public FromTo<ulong> Value { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        protected Offset()
        {
            Value = new FromTo<ulong>()
            {
                From = ulong.MinValue,
                To = ulong.MaxValue
            };
        }

        public override string ToString()
        {
            return Value == null ? "NULL" : Value.ToString();
        }
    }
}