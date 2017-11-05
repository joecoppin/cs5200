using System.Collections.Concurrent;
using CommSubsystem.Conversations;
using SharedObjects;

using log4net;

namespace CommSubsystem
{
    public class ConversationDictionary
    {
        #region Private Class Members
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConversationDictionary));

        // Create a dictionary of queues for conversations in progress, plus a lock object for the dictionary
        private readonly ConcurrentDictionary<MessageId, Conversation> _conversations =
            new ConcurrentDictionary<MessageId, Conversation>(new MessageId.MessageIdComparer());

        #endregion

        #region Public Methods

        public bool Add(MessageId convId, Conversation conversation)
        {
            if (convId == null) return false;
            if (Lookup(convId) != null) return true;

            Logger.Debug($"Try to add conversation with id for convId={convId}");
            _conversations.TryAdd(convId, conversation);

            return true;
        }

        public Conversation Lookup(MessageId convId)
        {
            Logger.Debug($"Lookup conversation for convId={convId}");

            Conversation result;
            _conversations.TryGetValue(convId, out result);

            return result;
        }

        public bool Remove(MessageId convId)
        {
            Logger.DebugFormat("Remove Queue {0}", convId);
            Conversation conversation;
            _conversations.TryRemove(convId, out conversation);
            return conversation != null;
        }

        public void ClearAll()
        {
            _conversations.Clear();
        }

        public int ConversationCount => _conversations.Count;

        #endregion
    }
}
