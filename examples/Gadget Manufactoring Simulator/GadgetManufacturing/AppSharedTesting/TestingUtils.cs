using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSharedTesting
{
    public class TestingUtils
    {
        [AssemblyInitialize]
        public static void InitializeTesting(TestContext context)
        {
            XmlConfigurator.Configure();
        }
    }
}
