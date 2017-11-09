using System;
using System.Windows.Forms;

using log4net.Config;

namespace Registry
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RuntimeOptions options = new RuntimeOptions();
            CommandLine.Parser parser = new CommandLine.Parser(with => with.HelpWriter = Console.Out);

            if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-2)))
            {
                options.SetDefaults();

                Registry registry = new Registry() { Options = options, Label = options.Label };
                registry.Start();

                RegistryDisplay displayForm = new RegistryDisplay() { MyProcess = registry };

                Application.Run(displayForm);

                registry.Stop();
            }
        }
    }
}
