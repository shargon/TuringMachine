using System.IO;

namespace TuringMachine.Core.Interfaces
{
    public interface IFuzzingInput: IType
    {
        /// <summary>
        /// Get Stream
        /// </summary>
        Stream GetStream();
        /// <summary>
        /// Type
        /// </summary>
        string Type { get; }
    }
}