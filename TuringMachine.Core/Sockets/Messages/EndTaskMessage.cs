using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TuringMachine.Core.Enums;
using TuringMachine.Core.FuzzingMethods.Patchs;
using TuringMachine.Core.Interfaces;
using TuringMachine.Core.Sockets.Enums;
using TuringMachine.Helpers;

namespace TuringMachine.Core.Sockets.Messages
{
    public class EndTaskMessage : TuringMessage
    {
        byte[] _Data;
        /// <summary>
        /// Extension
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data { get { return _Data; } set { _Data = value; } }
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

            if (Data != null && Data.Length > 0 && !string.IsNullOrEmpty(Extension))
            {
                ZipHelper.AppendOrCreateZip(ref _Data, GetLogsEntry(sender));

                // Save
                if (Data != null)
                {
                    string data = Path.Combine(Application.StartupPath, "Dumps", HashHelper.SHA1(Data) + ".zip");

                    // Create dir
                    if (!Directory.Exists(Path.GetDirectoryName(data)))
                        Directory.CreateDirectory(Path.GetDirectoryName(data));

                    // Write file
                    File.WriteAllBytes(data, Data);

                    return new FuzzerLog()
                    {
                        Input = StringHelper.List2String(sinput, "; "),
                        Config = StringHelper.List2String(sconfig, "; "),
                        Type = Result,
                        Origin = sender.EndPoint,
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

                    PatchConfig cfg = null;
                    if (o is FuzzingStream)
                    {
                        FuzzingStream f = (FuzzingStream)o;
                        cfg = new PatchConfig(f.SampleId, f.Log);
                    }
                    else
                    {
                        if (o is PatchConfig) cfg = (PatchConfig)o;
                        else
                        {
                            if (o is byte[] && key.StartsWith("Original"))
                            {
                                string iz, dr;
                                StringHelper.SplitInTwo(key, "=", out iz, out dr);

                                yield return new ZipHelper.FileEntry(dr + "_" + HashHelper.SHA1((byte[])o) + ".dat", (byte[])o);
                            }
                            else if (o is string && key.StartsWith("Info"))
                            {
                                string iz, dr;
                                StringHelper.SplitInTwo(key, "=", out iz, out dr);

                                byte[] data = Encoding.UTF8.GetBytes(o.ToString());
                                yield return new ZipHelper.FileEntry(dr + "_" + HashHelper.SHA1(data) + ".txt", data);
                            }
                        }
                    }

                    if (cfg != null)
                    {
                        string iz, dr;
                        StringHelper.SplitInTwo(key, "=", out iz, out dr);

                        byte[] bjson = Encoding.UTF8.GetBytes(cfg.ToJson());
                        yield return new ZipHelper.FileEntry(dr + "_" + HashHelper.SHA1(bjson) + ".fpatch", bjson);
                        continue;
                    }
                }
        }
    }
}