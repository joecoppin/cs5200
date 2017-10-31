using AppShared.Conversations.InitiatorConversations;
using Messages;

namespace GadgetAssembler
{
    public class ConversationFactory : CommSub.ConversationFactory
    {
        public override void Initialize()
        {
            Add(typeof(Shutdown), typeof(AppShared.Conversations.ResponderConversations.Shutdown));
        }
    }
}
