using System.IO;

namespace TuringMachine.Core
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
        public static void ReadFull(Stream stream, byte[] data, int index, int count)
        {
            while (count > 0)
            {
                int lee = stream.Read(data, index, count);
                index += lee;
                count -= lee;
            }
        }
    }
}