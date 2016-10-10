using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuringMachine.Core;
using TuringMachine.Core.FuzzingMethods.Mutational;
using TuringMachine.Core.FuzzingMethods.Patchs;

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
                AppendByte = new FromToValue<byte>((byte)'A'),
                RemoveLength = new FromToValue<ushort>(1),
                AppendLength = new FromToValue<ushort>(1),
            };

            PatchChange ret = c.Process(0);
            ret = c.Process(0);
            ret = c.Process(0);
        }
        [TestMethod]
        public void TestMutationalOffset()
        {
            MutationalOffset c = new MutationalOffset()
            {
                FuzzPercent = 5F,
                ValidOffset = new FromToValue<ulong>(0, ulong.MaxValue),
            };

            c.Changes.Add(new MutationalChange()
            {
                Weight = 9,
                AppendByte = new FromToValue<byte>((byte)'A'),
                RemoveLength = new FromToValue<ushort>(5),
            });
            c.Changes.Add(new MutationalChange()
                        {
                           // Remmove
                            Weight=1,
                            RemoveLength = new FromToValue<ushort>(1),
                            AppendLength=new FromToValue<ushort>(0)
                        });

            for (int x = 0; x < 100; x++)
            {
                MutationalChange next = c.Get(0);
                if (next != null)
                {

                }
            }
        }
    }
}