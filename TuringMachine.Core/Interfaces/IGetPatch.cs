using TuringMachine.Core.FuzzingMethods.Patchs;

namespace TuringMachine.Core.Interfaces
{
    public interface IGetPatch
    {
        /// <summary>
        /// Get Patch
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="elapsedSeconds">Elapsed seconds</param>
        PatchChange Get(long offset, long elapsedSeconds);
    }
}