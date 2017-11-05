using CommandLine;
using CommSubsystem;

namespace WeatherStation
{
    public class WeatherStationRuntimeOptions : RuntimeOptions
    {

        [Option("pMinutes", MetaValue = "INT", Required = false, HelpText = "Minutes between publishing weather data")]
        public int? PublicationIntervalNullable { get; set; }
        public int PublicationInterval => PublicationIntervalNullable ?? 0;


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

            if (PublicationIntervalNullable == null)
                PublicationIntervalNullable = Properties.Settings.Default.PublicationInterval;

            if (DsDiscoveryIntervalNullable == null)
                DsDiscoveryIntervalNullable = Properties.Settings.Default.DiscoveryInterval;
        }
    }
}
