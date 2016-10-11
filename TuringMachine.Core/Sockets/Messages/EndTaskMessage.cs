using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets.Enums;
using TuringMachine.Helpers;

namespace TuringMachine.Core.Sockets.Messages
{
    public class EndTaskMessage : TuringMessage
    {
        byte[] _ZipData;
        /// <summary>
        /// Data
        /// </summary>
        public byte[] ZipData { get { return _ZipData; } set { _ZipData = value; } }
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
        /// <param name="sender">Socket</param>
        public FuzzerLog SaveResult(TuringSocket sender, out List<FuzzerStat<IFuzzingInput>> sinput, out List<FuzzerStat<IFuzzingConfig>> sconfig)
        {
            sinput = (List<FuzzerStat<IFuzzingInput>>)sender["INPUT"];
            sconfig = (List<FuzzerStat<IFuzzingConfig>>)sender["CONFIG"];

            if (ZipData != null && ZipData.Length > 0)
            {
                ZipHelper.AppendOrCreateZip(ref _ZipData, GetLogsEntry(sender));

                // Save
                if (ZipData != null)
                {
                    string data = Path.Combine(Application.StartupPath, "Dumps", HashHelper.SHA1(ZipData) + ".zip");

                    // Create dir
                    if (!Directory.Exists(Path.GetDirectoryName(data)))
                        Directory.CreateDirectory(Path.GetDirectoryName(data));

                    // Write file
                    File.WriteAllBytes(data, ZipData);

                    return new FuzzerLog()
                    {
                        Input = StringHelper.List2String(sinput, "; "),
                        Config = StringHelper.List2String(sconfig, "; "),
                        Type = Result,
                        Origin = sender.EndPoint.Address,
                        Path = data
                    };
                }
            }

            return null;
        }
        /// <summary>
        /// Search in socket variables
        /// </summary>
        /// <param name="socket">Socket</param>
        IEnumerable<ZipHelper.FileEntry> GetLogsEntry(TuringSocket socket)
        {
            if (socket != null)
                foreach (string key in socket.Variables.Key)
                {
                    object o = socket.Variables[key];
                    if (o == null) continue;

                    FuzzingLogInfo cfg = null;
                    if (o is FuzzingStream)
                    {
                        FuzzingStream f = (FuzzingStream)o;
                        cfg = new FuzzingLogInfo(f);
                    }
                    else
                    {
                        if (o is FuzzingLogInfo) cfg = (FuzzingLogInfo)o;
                    }

                    if (cfg != null)
                    {
                        string iz, dr;
                        StringHelper.SplitInTwo(key, "=", out iz, out dr);

                        // Save original input
                        if (cfg.OriginalData != null)
                            yield return new ZipHelper.FileEntry(dr + "_" + HashHelper.SHA1(cfg.OriginalData) + ".dat", cfg.OriginalData);

                        if (!string.IsNullOrEmpty(cfg.Info))
                        {
                            // Save info
                            byte[] data = Encoding.UTF8.GetBytes(cfg.Info);
                            yield return new ZipHelper.FileEntry(dr + "_" + HashHelper.SHA1(data) + ".txt", data);
                        }

                        if (cfg.Patch != null)
                        {
                            // Save patch
                            byte[] bjson = Encoding.UTF8.GetBytes(cfg.Patch.ToJson());
                            yield return new ZipHelper.FileEntry(dr + "_" + HashHelper.SHA1(bjson) + ".fpatch", bjson);
                        }
                    }
                }
        }
    }
}