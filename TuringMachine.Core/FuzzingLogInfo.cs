using TuringMachine.Core.FuzzingMethods.Patchs;

namespace TuringMachine.Core
{
    public class FuzzingLogInfo
    {
        /// <summary>
        /// Info
        /// </summary>
        public string Info { get; private set; }
        /// <summary>
        /// Original data
        /// </summary>
        public byte[] OriginalData { get; private set; }
        /// <summary>
        /// Patch
        /// </summary>
        public PatchConfig Patch { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fs">Stream</param>
        public FuzzingLogInfo(FuzzingStream fs)
        {
            if (fs == null) return;

            Info = "";
            if (!string.IsNullOrEmpty(fs.InputName)) Info = "Input: " + fs.InputName;
            if (!string.IsNullOrEmpty(fs.ConfigName)) Info += (Info != "" ? "\n" : "") + "Config: " + fs.ConfigName;

            OriginalData = fs.OriginalData;

            if (fs.Log != null && fs.Log.Length > 0)
                Patch = new PatchConfig(fs.SampleId.ToString(), fs.Log);
        }
    }
}