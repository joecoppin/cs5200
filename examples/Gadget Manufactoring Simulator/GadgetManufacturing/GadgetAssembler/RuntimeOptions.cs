using CommandLine;

namespace GadgetAssembler
{
    public class RuntimeOptions : CommSub.RuntimeOptions
    {
        [Option("registry", MetaValue = "STRING", Required = false, HelpText = "Registry's public hostname and port")]
        public string RegistryHostAndPort { get; set; }

        public override void SetDefaults()
        {
            if (string.IsNullOrWhiteSpace(RegistryHostAndPort))
                RegistryHostAndPort = Properties.Settings.Default.RegistryHostAndPort;

            if (MinPortNullable == null)
                MinPortNullable = Properties.Settings.Default.MinPort;

            if (MaxPortNullable == null)
                MaxPortNullable = Properties.Settings.Default.MaxPort;

            if (TimeoutNullable == null)
                TimeoutNullable = Properties.Settings.Default.Timeout;

            if (RetriesNullable == null)
                RetriesNullable = Properties.Settings.Default.MaxRetries;
        }
    }
}
