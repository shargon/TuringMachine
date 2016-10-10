using System;

namespace TuringMachine.Core.FuzzingMethods.Patchs
{
    public class PatchChange
    {
        /// <summary>
        /// Offset
        /// </summary>
        public long Offset { get; set; }
        /// <summary>
        /// Remove
        /// </summary>
        public ushort Remove { get; set; }
        /// <summary>
        /// Append
        /// </summary>
        public byte[] Append { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PatchChange() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="remove">Remove</param>
        public PatchChange(long offset, ushort remove)
        {
            Offset = offset;
            Remove = remove;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="append">Append</param>
        /// <param name="remove">Remove</param>
        public PatchChange(long offset, byte[] append, ushort remove)
        {
            Offset = offset;
            Append = append;
            Remove = remove;
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return
                Offset.ToString() +
                " - Remove: " + Remove.ToString() +
                (Append == null || Append.Length <= 0 ? "" : " - Append: " + Convert.ToBase64String(Append));
        }
    }
}