using System.Security.Cryptography;
using System.Text;

namespace TuringMachine.Helpers
{
    public class HashHelper
    {
        /// <summary>
        /// Get SHA1 of binary data
        /// </summary>
        /// <param name="data">Input</param>
        public static string SHA1(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA1 sha1 = SHA1Managed.Create())
            {
                byte[] stream = sha1.ComputeHash(data);
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            }
            return sb.ToString();
        }
    }
}