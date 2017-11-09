using SharedObjects;
using System.Net;
using System.Net.NetworkInformation;
using log4net;

namespace CommSub
{
    /// <summary>
    /// CommSubsystem
    /// 
    /// A CommSubsystem is a facade that encapsulates a CommProcessState, UdpCommunication, QueueDictionary, and ConversationFactory
    /// </summary>
    public class CommSubsystem
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CommSubsystem));

        private UdpCommunicator _myUdpCommunicator;
        private IPAddress _bestAddress;
        private readonly QueueDictionary _queueDictionary;
        private readonly ConversationFactory _conversationFactory;
        private readonly CommProcessState _processState;

        public CommProcessState ProcessState => _processState;
        public int Port => _myUdpCommunicator?.Port ?? 0;
        public string BestLocalEndPoint => $"{FindBestLocalIpAddress()}:{Port}"; 

        public CommSubsystem(CommProcessState processState, ConversationFactory factory)
        {
            _processState = processState;
            _conversationFactory = factory;
            _conversationFactory.ManagingSubsystem = this;
            _queueDictionary = new QueueDictionary();
        }

        /// <summary>
        /// This methods setup up all of the components in a CommSubsystem.  Call this method
        /// sometime after setting the MinPort, MaxPort, and ConversationFactory
        /// </summary>
        public void Initialize()
        {
            _conversationFactory.Initialize();

            _myUdpCommunicator = new UdpCommunicator()
                                    {
                                        MinPort = ProcessState.Options.MinPort,
                                        MaxPort = ProcessState.Options.MaxPort,
                                        Timeout = ProcessState.Options.Timeout,
                                        LoggerPrefix = ProcessState.Options.Label,
                                        EnvelopeHandler = ProcessIncomingEnvelope
                                    };

            _processState.State = CommProcessState.PossibleState.Initialized;
        }
        
        /// <summary>
        /// This method starts up all active components in the CommSubsystem.  Call this method
        /// sometime after calling Initalize.
        /// </summary>
        public void Start()
        {
            Logger.Debug("Entering Start");
            _myUdpCommunicator.Start();
            Logger.Debug("Leaving Start");
        }

        /// <summary>
        /// This method stops all of the active components of a CommSubsystem and release the
        /// releases (or at least allows them to be garabage collected.  Once stop is called,
        /// a CommSubsystem cannot be restarted with setting it up from scratch.
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

        public virtual T CreateFromConversationType<T>() where T : Conversation, new()
        {
            return _conversationFactory.CreateFromConversationType<T>();
        }

        public ConversationQueue SetupConversationQueue(MessageId convId)
        {
            return _queueDictionary.CreateQueue(convId);
        }

        public void CloseConversationQueue(MessageId convId)
        {
            _queueDictionary.CloseQueue(convId);
        }

        public Error Send(Envelope env)
        {
            return _myUdpCommunicator.Send(env);
        }

        public void ProcessIncomingEnvelope(Envelope env)
        {
            if (env == null) return;

            ConversationQueue existConversationQueue = _queueDictionary.Lookup(env.Message.ConvId);
            if (existConversationQueue != null)
                existConversationQueue.Enqueue(env);
            else
            {
                Conversation conv = _conversationFactory.CreateFromMessage(env);
                conv?.Launch();
            }
        }

        public void SetupMulticastAddress(string groupAddress)
        {
            
        }

        private IPAddress FindBestLocalIpAddress()
        {
            if (_bestAddress != null) return _bestAddress;
            
            long bestPreferredLifetime = 0;
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (_bestAddress == null || ip.AddressPreferredLifetime > bestPreferredLifetime)
                            {
                                _bestAddress = ip.Address;
                                bestPreferredLifetime = ip.AddressPreferredLifetime;
                            }
                        }
                    }
                }
            }
            return _bestAddress;
        }
    }
}
