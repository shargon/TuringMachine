using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TuringMachine.Helpers
{
    public class ZipHelper
    {
        public class FileEntry
        {
            /// <summary>
            /// Data
            /// </summary>
            public byte[] Data { get; set; }
            /// <summary>
            /// Name
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="fileName">FileName</param>
            /// <param name="data">Data</param>
            public FileEntry(string fileName, byte[] data)
            {
                FileName = fileName;
                Data = data;
            }
            /// <summary>
            /// String representation
            /// </summary>
            public override string ToString() { return FileName; }
        }
        /// <summary>
        /// Append or create a Zip
        /// </summary>
        /// <param name="zip">Current zip</param>
        /// <param name="append">Append</param>
        public static int AppendOrCreateZip(ref byte[] zip, IEnumerable<FileEntry> append)
        {
            bool currentIsZip = zip != null && zip.Length > 0;

            int dv = 0;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Append zip
                if (currentIsZip)
                {
                    memoryStream.Write(zip, 0, zip.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }
                // Append entries
                using (ZipArchive archive = new ZipArchive(memoryStream, currentIsZip ? ZipArchiveMode.Update : ZipArchiveMode.Create, true))
                {
                    foreach (FileEntry f in append)
                    {
                        using (Stream entryStream = archive.CreateEntry(f.FileName).Open())
                            entryStream.Write(f.Data, 0, f.Data.Length);
                        dv++;
                    }
                }

                // Recover zip
                memoryStream.Seek(0, SeekOrigin.Begin);
                zip = memoryStream.ToArray();
            }

            return dv;
        }
    }
}