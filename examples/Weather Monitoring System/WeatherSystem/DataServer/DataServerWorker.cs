using System;
using System.Threading;
using CommSubsystem;
using SharedAppLayer;

namespace DataServer
{
    public class DataServerWorker : WmsAppWorker
    {
        private DateTime _lastSync = DateTime.MinValue;

        protected override void Run()
        {
            var options = Options as DataServerRuntimeOptions;
            var currentTime = DateTime.Now;

            while (KeepGoing)
            {
                DataServerDiscovery();
                SyncWithOtherDataServers();
                Thread.Sleep(100);
            }
        }

        protected override ConversationFactory CreateConversationFactory()
        {
            return new DataServerConversationFactory();
        }


        protected void SyncWithOtherDataServers()
        {
            var options = Options as DataServerRuntimeOptions;
            var currentTime = DateTime.Now;
            if (currentTime.Subtract(_lastSync).Minutes > options?.SyncInterval)
            {

                // TODO: Implement

                _lastSync = currentTime;
            }

        }
    }
}
