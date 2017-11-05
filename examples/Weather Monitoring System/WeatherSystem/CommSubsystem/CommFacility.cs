using CommSubsystem.Conversations;
using SharedObjects;
using log4net;

namespace CommSubsystem
{
    public class CommFacility
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CommFacility));

        private readonly ConversationFactory _conversationFactory;
        private readonly ConversationDictionary _conversationDictionary;
        private UdpCommunicator _myUdpCommunicator;

        public AppWorker AppProcess { get; }

        public CommFacility(AppWorker appProcess, ConversationFactory factory)
        {
            AppProcess = appProcess;
            _conversationFactory = factory;
            _conversationFactory.ManagingCommFacility = this;
            _conversationDictionary = new ConversationDictionary();
        }

        /// <summary>
        /// This methods setup up all of the components in a CommFacility.  Call this method
        /// sometime after setting the MinPort, MaxPort, and ConversationFactory
        /// </summary>
        public void Initialize()
        {
            _conversationFactory.Initialize();

            _myUdpCommunicator = new UdpCommunicator() 
            {
                MinPort = AppProcess.Options.MinPort,
                MaxPort = AppProcess.Options.MaxPort,
                Timeout = AppProcess.Options.Timeout,
                EnvelopeHandler = DelegateToConversation
            };

        }

        /// <summary>
        /// This method starts up all active components in the CommFacility.  Call this method
        /// sometime after calling Initalize.
        /// </summary>
        public void Start()
        {
            Logger.Debug("Entering Start");
            _myUdpCommunicator.Start();
            Logger.Debug("Leaving Start");
        }

        /// <summary>
        /// This method stops all of the active components of a CommFacility and release the
        /// releases (or at least allows them to be garabage collected.  Once stop is called,
        /// a CommFacility cannot be restarted with setting it up from scratch.
        /// </summary>
        public void Stop()
        {
            Logger.Debug("Entering Stop");

            if (_myUdpCommunicator != null)
            {
                _myUdpCommunicator.Stop();
                _myUdpCommunicator = null;
            }

            Logger.Debug("Leaving Stop");
        }

        public Error Send(Envelope env)
        {
            return _myUdpCommunicator.Send(env);
        }

        public void DelegateToConversation(Envelope envelope)
        {
            if (envelope == null) return;

            var existConversation = _conversationDictionary.Lookup(envelope.Message.ConvId);
            if (existConversation != null)
                existConversation.Enqueue(envelope);
            else
            {
                var conv = _conversationFactory.Create(envelope);
                conv?.Launch();
            }
        }

        public void CreateConversationQueue(MessageId queueId, Conversation conv)
        {
            _conversationDictionary.Add(queueId, conv);
        }

    }
}
