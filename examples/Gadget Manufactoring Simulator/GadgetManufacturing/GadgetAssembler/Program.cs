using System;
using System.Windows.Forms;

using AppShared;
using log4net.Config;

namespace GadgetAssembler
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

                GadgetAssembler gadgetAssembler = new GadgetAssembler() { Options = options, Label = options.Label };
                gadgetAssembler.Start();

                AppProcessDisplayForm displayForm = new AppProcessDisplayForm() { MyProcess = gadgetAssembler };

                Application.Run(displayForm);

                gadgetAssembler.Stop();
            }
        }
    }
}
