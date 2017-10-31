using CommSub;
using SharedObjects;

using AppShared.Conversations.InitiatorConversations;
using log4net;

namespace AppShared
{
    public abstract class AppProcess : CommProcess
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AppProcess));
        public PublicEndPoint RegistryEndPoint { get; set; }

        public RuntimeOptions MyOptions => Options as RuntimeOptions;

        public short ProcessId => LocalProcessInfo.Instance.ProcessId;

        public abstract ProcessType ProcessType { get; }

        protected void DoRegistration()
        {
            Registration conv = MyCommSubsystem.CreateFromConversationType<Registration>();
            conv.RemotEndPoint = RegistryEndPoint;
            conv.Execute();
        }


    }
}
