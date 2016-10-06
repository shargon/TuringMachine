namespace TuringMachine.Helpers.Interfaces
{
    public interface IRandomValue<T>
    {
        /// <summary>
        /// Get next value
        /// </summary>
        T Get();
    }
}