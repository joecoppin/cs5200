using CommSub;
using Messages;

namespace Registry
{
    public class ConversationFactory : CommSub.ConversationFactory
    {
        public override void Initialize()
        {
            Add(typeof(Register), typeof(Conversations.ResponderConversations.Registration));
        }
    }
}
