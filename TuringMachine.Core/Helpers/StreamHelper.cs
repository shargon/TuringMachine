using System.IO;

namespace TuringMachine.Core.Helpers
{
    public class StreamHelper
    {
        /// <summary>
        /// Read all data
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="count">Count</param>
        public static int ReadFull(Stream stream, byte[] data, int index, int count)
        {
            int total = 0;
            while (count > 0)
            {
                int lee = stream.Read(data, index, count);
                if (lee <= 0)
                    break;

                index += lee;
                count -= lee;
                total += lee;
            }
            return total;
        }
        /// <summary>
        /// Read all data
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="data">Data</param>
        /// <param name="index">Index</param>
        /// <param name="count">Count</param>
        public static int ReadFull(Stream stream, byte[] data, ref int index, int count)
        {
            int total = 0;
            while (count > 0)
            {
                int lee = stream.Read(data, index, count);
                if (lee <= 0)
                    break;

                index += lee;
                count -= lee;
                total += lee;
            }
            return total;
        }
    }
}