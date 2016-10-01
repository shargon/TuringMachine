namespace TuringMachine.Core.Interfaces
{
    public interface IFuzzingConfig: IType
    {
        /// <summary>
        /// Serialize to Json
        /// </summary>
        string ToJson();
    }
}