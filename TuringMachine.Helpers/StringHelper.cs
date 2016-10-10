using System;
using System.Collections;
using System.Collections.Generic;

namespace TuringMachine.Helpers
{
    public class StringHelper
    {
        /// <summary>
        /// Split input in two string
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="sep">Separator</param>
        /// <param name="left">Left variable</param>
        /// <param name="right">Right variable</param>
        /// <returns>Returns false if sep dosen't appear</returns>
        public static bool SplitInTwo(string input, string sep, out string left, out string right)
        {
            int fi = string.IsNullOrEmpty(input) ? -1 : input.IndexOf(sep);
            if (fi == -1) { left = input; right = ""; return false; }

            left = input.Substring(0, fi);
            int sl = sep.Length;
            right = input.Substring(fi + sl, input.Length - fi - sl);
            return true;
        }
        /// <summary>
        /// Convert list to string
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="separator">Separator</param>
        public static string List2String<T>(IList<T> list, string separator)
        {
            string dv = "";

            if (list != null)
                foreach (T o in list)
                {
                    if (o == null) continue;
                    if (dv != "") dv += separator;
                    dv += o.ToString();
                }

            return dv;
        }
    }
}