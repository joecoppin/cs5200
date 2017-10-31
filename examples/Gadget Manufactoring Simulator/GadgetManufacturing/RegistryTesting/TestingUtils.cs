﻿using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedObjects;

namespace RegistryTesting
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
