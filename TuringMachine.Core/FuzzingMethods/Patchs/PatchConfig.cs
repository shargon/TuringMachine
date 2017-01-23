using System.Collections.Generic;
using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Core.FuzzingMethods.Patchs
{
    public class PatchConfig : IFuzzingConfig
    {
        /// <summary>
        /// Mutations
        /// </summary>
        [TypeConverter(typeof(ListArrayReadOnlyConverter))]
        public List<PatchChange> Changes { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Patch"; } }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public PatchConfig()
        {
            Changes = new List<PatchChange>();
            Description = "Unnamed";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="description">Description</param>
        /// <param name="changes">Changes</param>
        public PatchConfig(string description, PatchChange[] changes) : this()
        {
            Description = description;
            Changes.AddRange(changes);
        }

        /// <summary>
        /// Deserialize from Json
        /// </summary>
        /// <param name="json">Json</param>
        public static PatchConfig FromJson(string json)
        {
            return SerializationHelper.DeserializeFromJson<PatchConfig>(json);
        }
        /// <summary>
        /// Convert to Json
        /// </summary>
        public string ToJson()
        {
            return SerializationHelper.SerializeToJson(this, true);
        }
        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return Description;
        }
        /// <summary>
        /// Get fixed patch
        /// </summary>
        /// <param name="stream">Stream</param>
        public PatchChange Get(FuzzingStream stream)
        {
            long offset = stream.Position;

            foreach (PatchChange p in Changes)
                if (p.Offset == offset)
                    return p;

            return null;
        }
        public void InitFor(FuzzingStream stream) { }
    }
}