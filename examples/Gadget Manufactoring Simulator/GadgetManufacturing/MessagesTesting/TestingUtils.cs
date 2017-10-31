using Microsoft.VisualStudio.TestTools.UnitTesting;

using log4net.Config;
using SharedObjects;

namespace MessagesTesting
{
    [TestClass]
    public class TestingUtils
    {
        [AssemblyInitialize]
        public static void InitializeTesting(TestContext context)
        {
            XmlConfigurator.Configure();

            LocalProcessInfo.Instance.ProcessId = 100;
        }
    }
}
