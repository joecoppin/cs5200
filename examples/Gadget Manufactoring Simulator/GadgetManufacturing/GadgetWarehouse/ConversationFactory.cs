using AppShared.Conversations.InitiatorConversations;
using Messages;

namespace GadgetWarehouse
{
    public class ConversationFactory : CommSub.ConversationFactory
    {
        public override void Initialize()
        {
            Add(typeof(Shutdown), typeof(AppShared.Conversations.ResponderConversations.Shutdown));
        }
    }
}
