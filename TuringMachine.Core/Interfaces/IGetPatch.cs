using TuringMachine.Core.FuzzingMethods.Patchs;

namespace TuringMachine.Core.Interfaces
{
    public interface IGetPatch
    {
        /// <summary>
        /// Get Patch
        /// </summary>
        /// <param name="offset">Offset</param>
        PatchChange Get(long offset);
    }
}