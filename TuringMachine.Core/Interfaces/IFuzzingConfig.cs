using System.IO;

namespace TuringMachine.Core.Interfaces
{
    public interface IFuzzingConfig : IType
    {
        /// <summary>
        /// Serialize to Json
        /// </summary>
        string ToJson();
        /// <summary>
        /// Create a Fuzzer Stream
        /// </summary>
        /// <param name="original">Original stream</param>
        /// <param name="sampleId">Sample Id</param>
        FuzzingStream CreateStream(Stream original, string sampleId);
    }
}