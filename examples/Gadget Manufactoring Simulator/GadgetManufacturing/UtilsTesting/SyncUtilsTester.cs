using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Utils;

namespace UtilsTesting
{
    [TestClass]
    public class SyncUtilsTester
    {
        private bool _flag;

        [TestMethod]
        public void SyncUtils_TestEverything()
        {
            // Case 0 - Timeout will occur
            DateTime ts = DateTime.Now;
            bool result = SyncUtils.WaitForCondition(CheckFlag, 4000);
            double netTime = DateTime.Now.Subtract(ts).TotalMilliseconds;
            Assert.IsFalse(netTime < 4000);
            Assert.IsFalse(result);

            // Case 1 - Condition will be met before timeout
            _flag = false;
            var timer = new Timer(SetFlag, true, 1000, Timeout.Infinite);

            ts = DateTime.Now;
            result = SyncUtils.WaitForCondition(CheckFlag, 4000);
            netTime = DateTime.Now.Subtract(ts).TotalMilliseconds;
            Assert.IsTrue(netTime<4000);
            Assert.IsTrue(result);

            timer.Dispose();

            // Case 2 - Zero timeout with a false condition
            _flag = false;
            timer = new Timer(SetFlag, true, 100, Timeout.Infinite);

            ts = DateTime.Now;
            result = SyncUtils.WaitForCondition(CheckFlag, 0);
            netTime = DateTime.Now.Subtract(ts).TotalMilliseconds;
            Assert.IsTrue(netTime < 100);
            Assert.IsFalse(result);

            timer.Dispose();

            // Case 3 - Zero timeout with a true condition
            _flag = true;
            timer = new Timer(SetFlag, false, 100, Timeout.Infinite);

            ts = DateTime.Now;
            result = SyncUtils.WaitForCondition(CheckFlag, 0);
            netTime = DateTime.Now.Subtract(ts).TotalMilliseconds;
            Assert.IsTrue(netTime < 100);
            Assert.IsTrue(result);

            timer.Dispose();
        }

        private void SetFlag(object state)
        {
            _flag = (bool) state;
        }

        private bool CheckFlag()
        {
            return _flag == true; 
        }
    }
}
