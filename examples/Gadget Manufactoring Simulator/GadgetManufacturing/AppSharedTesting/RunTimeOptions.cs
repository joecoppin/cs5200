
using CommandLine;

namespace AppSharedTesting
{
    public class RuntimeOptions : CommSub.RuntimeOptions
    {
        public string RegistryHostAndPort { get; set; }

        public RuntimeOptions(int registryPort)
        {
            RegistryHostAndPort = $"127.0.0.1:{registryPort}";
            MinPortNullable = 12001;
            MaxPortNullable = 12099;
            TimeoutNullable = 1000;
            RetriesNullable = 3;
        }

        public override void SetDefaults()
        {
        }
    }
}
