using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedObjects;

namespace CommSubsystemTest
{
    [TestClass]
    public class TestUtils
    {
        [TestInitialize]
        public void SetupTesting()
        {
            LocalProcessInfo.Instance.ProcessId = 10;
        }

    }
}
