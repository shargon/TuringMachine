using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TuringMachine.Core.Detectors.Windows;
using TuringMachine.Core.Enums;

namespace TuringMachine.Tests
{
    [TestClass]
    public class DetectorTests
    {
        [TestMethod]
        public void TestWerDetector()
        {
            WERDetector w = new WERDetector(new ProcessStartInfo(@"D:\CrashDumps\test.exe"));

            byte[] data;
            EExploitableResult res;
            w.IsCrashed(null, out data, out res, null, null);
        }
    }
}