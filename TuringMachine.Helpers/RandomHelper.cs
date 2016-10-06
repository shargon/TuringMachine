using System;
using TuringMachine.Helpers.Interfaces;

namespace TuringMachine.Helpers
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
        public static byte GetRandom(byte from, byte to)
        {
            if (from == to) return to;

            return (byte)_Rand.Next(from, to + 1);
        }
        /// <summary>
        /// Return next byte
        /// </summary>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        /// <param name="excludes">Excludes</param>
        public static ushort GetRandom(ushort from, ushort to)
        {
            if (from == to) return to;

            return (ushort)_Rand.Next(from, (to + 1));
        }
        /// <summary>
        /// Return next int
        /// </summary>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        public static int GetRandom(int from, int to)
        {
            if (from == to) return to;

            return _Rand.Next(from, to + 1);
        }
        /// <summary>
        /// Return next long
        /// </summary>
        /// <param name="from">From byte</param>
        /// <param name="to">To byte</param>
        public static long GetRandom(long from, long to)
        {
            if (from == to) return to;

            byte[] buf = new byte[8];
            _Rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (to - from)) + from);
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
        public static void Randomize<T>(T[] buffer, int index, int length, IRandomValue<T> get)
        {
            for (; length > 0; index++)
            {
                buffer[index] = get.Get();
                length--;
            }
        }
    }
}