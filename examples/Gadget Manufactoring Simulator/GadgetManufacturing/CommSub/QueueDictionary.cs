using System.Collections.Concurrent;

using SharedObjects;

using log4net;

namespace CommSub
{
    public class QueueDictionary
    {
        #region Private Class Members
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConversationQueue));

        // Create a dictionary of queues for conversations in progress, plus a lock object for the dictionary
        private readonly ConcurrentDictionary<MessageId, ConversationQueue> _activeQueues =
            new ConcurrentDictionary<MessageId, ConversationQueue>(new MessageId.MessageIdComparer());

        #endregion

        #region Public Methods

        public ConversationQueue CreateQueue(MessageId convId)
        {
            ConversationQueue result = null;
            if (convId != null)
            {
                Logger.DebugFormat("CreateQueue for key={0}", convId);
                result = Lookup(convId);
                if (result == null)
                {
                    result = new ConversationQueue() { QueueId = convId };
                    _activeQueues.TryAdd(convId, result);
                }
            }
            return result;
        }

        public ConversationQueue Lookup(MessageId convId)
        {
            Logger.DebugFormat("Lookup for name={0}", convId);

            ConversationQueue result;
            _activeQueues.TryGetValue(convId, out result);

            return result;
        }

        public void CloseQueue(MessageId queueId)
        {
            Logger.DebugFormat("Remove Queue {0}", queueId);
            ConversationQueue queue;
            _activeQueues.TryRemove(queueId, out queue);
        }

        public void ClearAllQueues()
        {
            _activeQueues.Clear();
        }

        public int ConversationQueueCount => _activeQueues.Count;

        #endregion
    }
}
