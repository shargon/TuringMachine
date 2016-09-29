namespace TuringMachine.Core
{
    public class Offset
    {
        /// <summary>
        /// Offset
        /// </summary>
        public FromTo<ulong> Value { get; set; }

        protected Offset()
        {
            Value = new FromTo<ulong>()
            {
                From = ulong.MinValue,
                To = ulong.MaxValue
            };
        }
    }
}