using System.Collections.Generic;
using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Core.Helpers;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Core.Mutational
{
    public class MutationConfig : IFuzzingConfig
    {
        /// <summary>
        /// Mutations
        /// </summary>
        [TypeConverter(typeof(ListArrayReadOnlyConverter))]
        public List<MutationalOffset> Mutations { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return "Mutational"; } }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MutationConfig()
        {
            Mutations = new List<MutationalOffset>();
            Description = "Unnamed";
        }
        /// <summary>
        /// Get next mutation
        /// </summary>
        /// <param name="offset">Offset</param>
        public MutationLog Get(long offset)
        {
            if (Mutations == null) return null;

            // Fuzzer
            foreach (MutationalOffset cond in Mutations)
            {
                if (!cond.ValidOffset.ItsValid((ulong)offset))
                    continue;

                // Try change
                MutationalChange change = cond.Get();
                if (change != null)
                    return change.Process(offset);
            }

            return null;
        }
        /// <summary>
        /// Deserialize from Json
        /// </summary>
        /// <param name="json">Json</param>
        public static MutationConfig FromJson(string json)
        {
            return SerializationHelper.DeserializeFromJson<MutationConfig>(json);
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
    }
}