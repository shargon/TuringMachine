using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TuringMachine.Client.Detectors.Windows;

namespace TuringMachine.Tests
{
    [TestClass]
    public class DetectorTests
    {
        [TestMethod]
        public void TestWerDetector()
        {
            WERDetector w = new WERDetector(new ProcessStartInfo(@"D:\CrashDumps\test.exe"));

            string ext;
            byte[] data;
            w.IsCrashed(null, out data, out ext, null);
        }
    }
}