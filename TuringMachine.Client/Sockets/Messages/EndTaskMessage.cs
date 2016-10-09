using System.IO;
using System.Windows.Forms;
using TuringMachine.Client.Sockets.Enums;
using TuringMachine.Helpers;

namespace TuringMachine.Client.Sockets.Messages
{
    public class EndTaskMessage : TuringMessage
    {
        /// <summary>
        /// Extension
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Result
        /// </summary>
        public EFuzzingReturn Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndTaskMessage() : base(ETuringMessageType.EndTask) { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value</param>
        public EndTaskMessage(EFuzzingReturn value) : this()
        {
            Result = value;
        }
        /// <summary>
        /// Save and return the save path
        /// </summary>
        public string SaveResult()
        {
            if (Data != null && Data.Length > 0 && !string.IsNullOrEmpty(Extension))
            {
                string data = Path.Combine(Application.StartupPath, "Dumps", HashHelper.SHA1(Data) + "." + Extension);

                // Create dir
                if (!Directory.Exists(Path.GetDirectoryName(data)))
                    Directory.CreateDirectory(Path.GetDirectoryName(data));

                // Write file
                File.WriteAllBytes(data, Data);
                return data;
            }
            return "";
        }
    }
}