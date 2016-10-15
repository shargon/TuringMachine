using System.Collections.Generic;
using System.ComponentModel;
using TuringMachine.Core.Design;
using TuringMachine.Core.FuzzingMethods.Patchs;
using TuringMachine.Core.Interfaces;
using TuringMachine.Helpers;

namespace TuringMachine.Core.FuzzingMethods.Mutational
{
    public class MutationConfig : IFuzzingConfig, IGetPatch
    {
        /// <summary>
        /// Mutations
        /// </summary>
        [TypeConverter(typeof(ListArrayReadOnlyConverter))]
        [Category("1 - Collection")]
        public List<MutationalOffset> Mutations { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [Category("2 - Info")]
        public string Type { get { return "Mutational"; } }
        /// <summary>
        /// Description
        /// </summary>
        [Category("2 - Info")]
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
        /// Init for
        /// </summary>
        /// <param name="stream">Stream</param>
        public void InitFor(FuzzingStream stream)
        {
            if (Mutations == null) return;

            int x = 0;
            foreach (MutationalOffset cond in Mutations)
            {
                cond.InitFor(stream, x);
                x++;
            }
        }
        /// <summary>
        /// Get next mutation
        /// </summary>
        /// <param name="stream">Stream</param>
        public PatchChange Get(FuzzingStream stream)
        {
            if (Mutations == null) return null;

            long offset = stream.Position;

            // Fuzzer
            int x = 0;
            foreach (MutationalOffset cond in Mutations)
            {
                if (!cond.ValidOffset.ItsValid((ulong)offset))
                {
                    x++;
                    continue;
                }

                // Try change
                MutationalChange change = cond.Get(stream, x);
                if (change != null) return change.Process(offset);
                x++;
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
        /// <summary>
        /// Create a Fuzzer Stream
        /// </summary>
        /// <param name="original">Original stream</param>
        public FuzzingStream CreateStream(byte[] original)
        {
            return new FuzzingStream(original, this);
        }
    }
}