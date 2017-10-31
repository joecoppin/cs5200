using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedObjects;

namespace SharedObjectsTesting
{
    [TestClass]
    public class LocalProcessInfoTester
    {
        [TestMethod]
        public void LocalProcessInfo_TestEverything()
        {
            LocalProcessInfo info1 = LocalProcessInfo.Instance;
            Assert.IsNotNull(info1);

            LocalProcessInfo info2 = LocalProcessInfo.Instance;
            Assert.IsNotNull(info2);
            Assert.AreSame(info1, info2);

            info1.ProcessId = 100;
            Assert.AreEqual(100, info1.ProcessId);

            info1.StartTime = DateTime.Now;
            TimeSpan span = DateTime.Now.Subtract(info1.StartTime);
            Assert.IsTrue(span.TotalMilliseconds < 100);
        }
    }
}
