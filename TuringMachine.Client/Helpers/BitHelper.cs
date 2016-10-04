using System;

namespace TuringMachine.Client.Helpers
{
    public class BitHelper
    {
        /// <summary>
        /// Get bytes of input
        /// </summary>
        /// <param name="value">Input</param>
        public static byte[] GetBytes(int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(buffer, 0, 4);
            return buffer;
        }
        /// <summary>
        /// Convert buffer to Int32
        /// </summary>
        /// <param name="value">Buffer</param>
        /// <param name="index">Index</param>
        public static int ToInt32(byte[] value, int index)
        {
            if (!BitConverter.IsLittleEndian)
                return BitConverter.ToInt32(new byte[] { value[index + 3], value[index + 2], value[index + 1], value[index] }, 0);
            return BitConverter.ToInt32(value, index);
        }
    }
}