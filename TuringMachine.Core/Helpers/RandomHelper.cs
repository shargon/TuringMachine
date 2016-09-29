using System;

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
        public static byte GetRandom(byte from, byte to, params byte[] excludes)
        {
            if (from == to) return to;

            byte r;
            do
            {
                r = (byte)_Rand.Next(from, to + 1);
                if (excludes != null)
                {
                    bool esta = false;

                    foreach (byte a in excludes)
                        if (a == r) { esta = true; }

                    if (esta) continue;
                }
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
        public static int GetRandom(int from, int to, params int[] excludes)
        {
            if (from == to) return to;

            int r;
            do
            {
                r = _Rand.Next(from, to + 1);
                if (excludes != null)
                {
                    bool esta = false;

                    foreach (int a in excludes)
                        if (a == r) { esta = true; }

                    if (esta) continue;
                }
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
        public static void Randomize(byte[] buffer, int index, int length, byte from, byte to, params byte[] excludes)
        {
            for (; length > 0; index++)
            {
                buffer[index] = GetRandom(from, to, excludes);
                length--;
            }
        }
    }
}