using CommSub;
using Messages;
using log4net;

namespace Registry.Conversations.InitialConversations
{
    public class Shutdown : InitiatorConversation
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Shutdown));

        protected override void ExecuteDetails(object context)
        {
            Logger.Debug("Entering ExecuteDetails");

            Error = CommSubsystem.Send(FirstEnvelope);

            // Since the Shutdown protocol is just a one-way (unreliable) send, this method doesn't
            // have to wait for any reply or do anything else besides remove the process from the
            // state as one of those registered.

            RegistryState state = CommSubsystem.ProcessState as RegistryState;
            state?.Unregister(RemotEndPoint);

            Logger.Debug("Leaving ExecuteDetails");
        }

        protected override ControlMessage CreateFirstMessage()
        {
            return new Messages.Shutdown();
        }
    }
}
