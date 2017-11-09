using System;

using CommSub;

namespace AppShared.Conversations.ResponderConversations
{
    public class Shutdown : ResponderConversation
    {
        protected override bool UseConverstationQueue => false;

        protected override void ExecuteDetails(object context)
        {
            CommSubsystem.ProcessState.State = CommProcessState.PossibleState.ReceivedShutdown;
        }
    }
}
