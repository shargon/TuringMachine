using System;
using System.Collections.Generic;

namespace TuringMachine.Core.Helpers
{
    public class RandomHelper
    {
        static Random _Rand = new Random();

        /// <summary>
        /// Return next byte
        /// </summary>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        /// <param name="excludes">Excludes</param>
        public static byte GetRandom(byte from, byte to, List<byte> excludes)
        {
            if (from == to) return to;

            byte r;
            do
            {
                r = (byte)_Rand.Next(from, to + 1);
                if (excludes != null && excludes.Contains(r)) continue;
                break;
            }
            while (true);
            return r;
        }
        /// <summary>
        /// Return next byte
        /// </summary>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        /// <param name="excludes">Excludes</param>
        public static ushort GetRandom(ushort from, ushort to, List<ushort> excludes)
        {
            if (from == to) return to;

            ushort r;
            do
            {
                r = (ushort)_Rand.Next(from, (to + 1));
                if (excludes != null && excludes.Contains(r)) continue;
                break;
            }
            while (true);
            return r;
        }
        /// <summary>
        /// Return next int
        /// </summary>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        /// <param name="excludes">Excludes</param>
        public static int GetRandom(int from, int to, List<int> excludes)
        {
            if (from == to) return to;

            int r;
            do
            {
                r = _Rand.Next(from, to + 1);
                if (excludes != null && excludes.Contains(r)) continue;
                break;
            }
            while (true);
            return r;
        }
        /// <summary>
        /// Randomize array
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="index">Index</param>
        /// <param name="length">Count</param>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        /// <param name="excludes">Excludes</param>
        public static void Randomize(byte[] buffer, int index, int length, byte from, byte to, List<byte> excludes)
        {
            for (; length > 0; index++)
            {
                buffer[index] = GetRandom(from, to, excludes);
                length--;
            }
        }
    }
}