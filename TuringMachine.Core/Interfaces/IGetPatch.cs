using TuringMachine.Core.FuzzingMethods.Patchs;

namespace TuringMachine.Core.Interfaces
{
    public interface IGetPatch
    {
        /// <summary>
        /// Get Patch
        /// </summary>
        /// <param name="stream">Stream</param>
        PatchChange Get(FuzzingStream stream);
        /// <summary>
        /// Init for this stream
        /// </summary>
        /// <param name="stream">Stream</param>
        void InitFor(FuzzingStream stream);
    }
}