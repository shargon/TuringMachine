using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuringMachine.Core;
using TuringMachine.Core.Mutational;
using TuringMachine.Core.Mutational.Changes;

namespace TuringMachine.Tests
{
    [TestClass]
    public class MutationalTests
    {
        [TestMethod]
        public void TestMutationalAppend()
        {
            MutationalAppend c = new MutationalAppend()
            {
                AppendByte = new Core.FromTo<byte>((byte)'A'),
                Count = new Core.FromTo<int>(5),
            };

            int l;
            byte[] ret = c.Process(out l);

        }
        [TestMethod]
        public void TestMutationalRemove()
        {
            MutationalRemove c = new MutationalRemove()
            {
                Count = new Core.FromTo<int>(5),
            };

            int l;
            byte[] ret = c.Process(out l);
        }
        [TestMethod]
        public void TestMutationalSwitch()
        {
            MutationalSwitch c = new MutationalSwitch()
            {
                AppendByte = new Core.FromTo<byte>((byte)'A'),
                Count = new Core.FromTo<int>(5),
            };

            int l;
            byte[] ret = c.Process(out l);

            ret = c.Process(out l);

            ret = c.Process(out l);
        }
        [TestMethod]
        public void TestOffset()
        {
            MutationalOffset c = new MutationalOffset()
            {
                FuzzPercent = 5F,
                Value = new FromTo<ulong>(0, ulong.MaxValue),
                Changes = new IMutationalChange[]
                   {
                       new MutationalSwitch()
                        {
                            Weight=9,
                            AppendByte = new Core.FromTo<byte>((byte)'A'),
                            Count = new Core.FromTo<int>(5),
                        },
                       new MutationalRemove()
                        {
                            Weight=1,
                            Count = new Core.FromTo<int>(5),
                        }
                   }
            };

            for (int x = 0; x < 100; x++)
            {
                IMutationalChange next = c.Get();
                if (next != null)
                {

                }
            }
        }
    }
}