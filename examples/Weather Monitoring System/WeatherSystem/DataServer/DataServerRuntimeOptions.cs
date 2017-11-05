using CommandLine;
using CommSubsystem;

namespace DataServer
{
    public class DataServerRuntimeOptions : RuntimeOptions
    {
        [Option("sMinutes", MetaValue = "INT", Required = false, HelpText = "Minutes between sync attempts")]
        public int? SyncIntervalNullable { get; set; }
        public int SyncInterval => SyncIntervalNullable ?? 0;

        public override void SetDefaults()
        {
            if (MinPortNullable == null)
                MinPortNullable = Properties.Settings.Default.MinPort;

            if (MaxPortNullable == null)
                MaxPortNullable = Properties.Settings.Default.MaxPort;

            if (TimeoutNullable == null)
                TimeoutNullable = Properties.Settings.Default.Timeout;

            if (MaxRetriesNullable == null)
                MaxRetriesNullable = Properties.Settings.Default.MaxRetries;

            if (string.IsNullOrWhiteSpace(DsGroupMultiCastAddress))
                DsGroupMultiCastAddress = Properties.Settings.Default.DsEndPoint;

            if (SyncIntervalNullable == null)
                SyncIntervalNullable = Properties.Settings.Default.SyncInterval;

            if (DsDiscoveryIntervalNullable == null)
                DsDiscoveryIntervalNullable = Properties.Settings.Default.DiscoveryInterval;

        }
    }
}
