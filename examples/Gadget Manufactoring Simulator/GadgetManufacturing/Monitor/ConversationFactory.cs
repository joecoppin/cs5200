using Messages;

namespace Monitor
{
    public class ConversationFactory : CommSub.ConversationFactory
    {
        public override void Initialize()
        {
            Add(typeof(Status), typeof(Conversations.ResponderConversations.Status));
        }
    }
}
