using System;
using System.Windows.Forms;
using log4net.Config;

namespace WeatherStation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var options = new WeatherStationRuntimeOptions();
            var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Out);

            if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-2)))
            {
                options.SetDefaults();

                var displayForm = new MainForm() { Options =  options };
                Application.Run(displayForm);
            }

            Application.Run(new MainForm());
        }
    }
}
