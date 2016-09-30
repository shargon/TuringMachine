using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuringMachine.Core;
using TuringMachine.Core.Mutational;

namespace TuringMachine.Tests
{
    [TestClass]
    public class MutationalTests
    {
        [TestMethod]
        public void TestMutationalChange()
        {
            MutationalChange c = new MutationalChange()
            {
                AppendByte = new Core.FromTo<byte>((byte)'A'),
                RemoveLength = new Core.FromTo<ushort>(1),
                AppendLength = new FromTo<ushort>(1),
            };

            MutationLog ret = c.Process(0);
            ret = c.Process(0);
            ret = c.Process(0);
        }
        [TestMethod]
        public void TestMutationalOffset()
        {
            MutationalOffset c = new MutationalOffset()
            {
                FuzzPercent = 5F,
                ValidOffset = new FromTo<ulong>(0, ulong.MaxValue),
            };

            c.Changes.AddRange(new MutationalChange[]
                       {
                       new MutationalChange()
                        {
                            Weight=9,
                            AppendByte = new Core.FromTo<byte>((byte)'A'),
                            RemoveLength = new Core.FromTo<ushort>(5),
                        },
                       new MutationalChange()
                        {
                           // Remmove
                            Weight=1,
                            RemoveLength = new Core.FromTo<ushort>(1),
                            AppendLength=new FromTo<ushort>(0)
                        }
                        });

            for (int x = 0; x < 100; x++)
            {
                MutationalChange next = c.Get();
                if (next != null)
                {

                }
            }
        }
    }
}