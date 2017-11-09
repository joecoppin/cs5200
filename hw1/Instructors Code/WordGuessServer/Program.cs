using System;
using System.ServiceProcess;

using log4net.Config;
using log4net;

namespace WordGuessServer
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

# if STANDALONE
            WordGuessService.StandAloneStartup(args);
# else
            Log.Debug("Setting ServicesToRun");
            var servicesToRun = new ServiceBase[] { new WordGuessService() };
            Log.Debug("Ready to Run");

            ServiceBase.Run(servicesToRun);
# endif
        }
    }
}
