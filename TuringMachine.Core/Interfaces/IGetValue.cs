using Newtonsoft.Json;
using System.ComponentModel;
using System.Drawing.Design;
using TuringMachine.Core.Design;

namespace TuringMachine.Core.Interfaces
{
    [JsonConverter(typeof(JsonFromToConverter))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Editor(typeof(IGetValueEditor), typeof(UITypeEditor))]
    public interface IGetValue<T>
    {
        /// <summary>
        /// Get next value
        /// </summary>
        T Get();
        /// <summary>
        /// Check if value its valid
        /// </summary>
        /// <param name="value">Value</param>
        bool ItsValid(T value);
        /// <summary>
        /// Class name
        /// </summary>
        string Name { get; }
    }
}