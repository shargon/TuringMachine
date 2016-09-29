using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuringMachine.Core.Mutational;

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
                Mode = MutationalAppend.EMode.AtEnd,
                OffsetAllowed = null
            };

            byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            int ix = 2, l = data.Length - ix;

            c.Process(ref data, ref ix, ref l, 0);

        }
        [TestMethod]
        public void TestMutationalRemove()
        {
            MutationalRemove c = new MutationalRemove()
            {
                Count = new Core.FromTo<int>(5),
                OffsetAllowed = null
            };

            byte[] data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int ix = 7, l = data.Length - ix;

            c.Process(ref data, ref ix, ref l, 0);
        }
        [TestMethod]
        public void TestMutationalSwitch()
        {
            MutationalSwitch c = new MutationalSwitch()
            {
                AppendByte = new Core.FromTo<byte>((byte)'A'),
                Count = new Core.FromTo<int>(5),
                OffsetAllowed = null
            };

            byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            int ix = 2, l = 3;

            c.Process(ref data, ref ix, ref l, 0);

            data = new byte[] { 1, 2, 3, 4, 5, 6 };
            ix = 1; l = 5;

            c.Process(ref data, ref ix, ref l, 0);

            data = new byte[] { 1, 2, 3, 4, 5, 6 };
            ix = 0; l = data.Length - ix;

            c.Process(ref data, ref ix, ref l, 0);
        }
    }
}
