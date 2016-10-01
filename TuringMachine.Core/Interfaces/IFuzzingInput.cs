using System.IO;

namespace TuringMachine.Core.Interfaces
{
    public interface IFuzzingInput : IType
    {
        /// <summary>
        /// Get Stream
        /// </summary>
        Stream GetStream();
        /// <summary>
        /// Is selectable
        /// </summary>
        bool IsSelectable { get; }
    }
}