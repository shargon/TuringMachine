namespace TuringMachine.Core.Interfaces
{
    public interface IFuzzingConfig : IType, IGetPatch
    {
        /// <summary>
        /// Serialize to Json
        /// </summary>
        string ToJson();
    }
}