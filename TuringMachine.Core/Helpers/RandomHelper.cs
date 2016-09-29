using System;

namespace TuringMachine.Core.Helpers
{
    public class RandomHelper
    {
        static Random _Rand = new Random();

        /// <summary>
        /// Return next byte
        /// </summary>
        public static byte GetRandom(byte from, byte to)
        {
            if (from == to) return to;
            return (byte)_Rand.Next(from, to + 1);
        }
        /// <summary>
        /// Return next int
        /// </summary>
        public static int GetRandom(int from, int to)
        {
            if (from == to) return to;
            return _Rand.Next(from, to + 1);
        }
        /// <summary>
        /// Randomize array
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="index">Index</param>
        /// <param name="length">Count</param>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        public static void Randomize(byte[] buffer, int index, int length, byte from, byte to)
        {
            for (; length > 0; index++)
            {
                buffer[index] = GetRandom(from, to);
                length--;
            }
        }
    }
}