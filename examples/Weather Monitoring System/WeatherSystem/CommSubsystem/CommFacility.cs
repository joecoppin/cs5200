using SharedObjects;
using System.Net;
using System.Net.NetworkInformation;
using log4net;

namespace CommSubsystem
{
    public class CommFacility
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CommFacility));

        private UdpCommunicator _myUdpCommunicator;
        private IPAddress _bestAddress;
        private readonly ConversationDictionary _conversationDictionary;
        private readonly ConversationFactory _conversationFactory;
        private readonly AppProcess _appProcess;

        public AppProcess AppProcess => _appProcess;
        public int Port => _myUdpCommunicator?.Port ?? 0;
        public string BestLocalEndPoint => $"{FindBestLocalIpAddress()}:{Port}";

        public CommFacility(AppProcess appProcess, ConversationFactory factory)
        {
            _appProcess = appProcess;
            _conversationFactory = factory;
            _conversationFactory.ManagingSubsystem = this;
            _conversationDictionary = new ConversationDictionary();
        }

        /// <summary>
        /// This methods setup up all of the components in a CommFacility.  Call this method
        /// sometime after setting the MinPort, MaxPort, and ConversationFactory
        /// </summary>
        public void Initialize()
        {
            _conversationFactory.Initialize();

            // TODO: Get setting from a command line via a settings object
            _myUdpCommunicator = new UdpCommunicator() 
            {
                MinPort = 12000,
                MaxPort = 19999,
                Timeout = 2000,
                LoggerPrefix = "",
                EnvelopeHandler = ProcessIncomingEnvelope
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

        public void ProcessIncomingEnvelope(Envelope env)
        {
            if (env == null) return;

            var existConversation = _conversationDictionary.Lookup(env.Message.ConvId);
            if (existConversation != null)
                existConversation.Enqueue(env);
            else
            {
                var conv = _conversationFactory.CreateFromMessage(env);
                conv?.Launch();
            }
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
