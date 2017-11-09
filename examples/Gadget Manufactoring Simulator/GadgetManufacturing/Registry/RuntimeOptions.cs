using CommandLine;

namespace Registry
{
    public class RuntimeOptions : CommSub.RuntimeOptions
    {
        [Option("publichost", MetaValue = "STRING", Required = false, HelpText = "Registry's public hostname")]
        public string PublicHostname { get; set; }

        public override void SetDefaults()
        {
            if (string.IsNullOrWhiteSpace(PublicHostname))
                PublicHostname = Properties.Settings.Default.PublicHostname;

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
