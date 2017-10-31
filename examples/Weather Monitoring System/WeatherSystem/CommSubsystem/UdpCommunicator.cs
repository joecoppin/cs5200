using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using System;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

using Messages;
using SharedObjects;
using log4net;

namespace CommSubsystem
{
    public delegate void IncomingEnvelopeHandler(Envelope env);

    public class UdpCommunicator
    {
        #region Private Data Members
        private UdpClient _myUdpClient;
        private bool _keepGoing;

        public IPAddress GroupAddress { get; set; }
        public int GroupPort { get; set; }
        public int TimeoutInMilliseconds { get; set; }

        private ILog _logger;
        private ILog _loggerDeep;
        private static readonly object StartStopLock = new object();
        #endregion

        #region Public Properties
        public int MinPort { get; set; }
        public int MaxPort { get; set; }
        public int Timeout { get; set; }
        public int Port => ((IPEndPoint)_myUdpClient?.Client.LocalEndPoint)?.Port ?? 0;
        public IncomingEnvelopeHandler EnvelopeHandler { get; set; }
        public string LoggerPrefix { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method starts up the UdpCommunicator by a) creating a UdpClient that is bound to a free port between the specified MinPort and MaxPort and b)
        /// starting a thread that looks for incoming messsages.  When that thread gets an incoming message, it will package it up in an envelope and callback
        /// to the specified EnvelopeHandler, which should be a method in the CommFacility
        /// </summary>
        public void Start()
        {
            LoggerPrefix = string.IsNullOrWhiteSpace(LoggerPrefix) ? "" : LoggerPrefix.Trim() + "_";

            _logger = LogManager.GetLogger(LoggerPrefix + typeof(UdpCommunicator));
            _loggerDeep = LogManager.GetLogger(LoggerPrefix + typeof(UdpCommunicator) + "_Deep");

            _logger.Info("Start communicator");

            ValidPorts();

            lock (StartStopLock)
            {
                if (_keepGoing) Stop();

                var portToTry = FindAvailablePort(MinPort, MaxPort);
                if (portToTry > 0)
                {
                    try
                    {
                        var localEp = new IPEndPoint(IPAddress.Any, portToTry);
                        _myUdpClient = new UdpClient(localEp);
                        Task.Factory.StartNew(Receive);
                        _keepGoing = true;
                    }
                    catch (SocketException)
                    {
                    }
                }

                if (!_keepGoing)
                    throw new ApplicationException($"Cannot bind the socket to a port {portToTry}");
            }
        }

        /// <summary>
        /// This method stops the UdpCommunicator by a) closing its UdpClient and b) setting stopping the receive thread.
        /// </summary>
        public void Stop()
        {
            _logger.Debug("Entering Stop");

            lock (StartStopLock)
            {
                _keepGoing = false;

                if (_myUdpClient != null)
                {
                    _myUdpClient.Close();
                    _myUdpClient = null;
                }
            }

            _logger.Info("Communicator Stopped");
        }

        /// <summary>
        /// This method sends the message in an envelope to the EndPoint of the envelope.  Note that just the message is
        /// send out the UdpClient, not the whole envelope.
        /// </summary>
        /// <param name="outgoingEnvelope"></param>
        /// <returns></returns>
        public Error Send(Envelope outgoingEnvelope)
        {
            Error error = null;
            if (outgoingEnvelope == null || !outgoingEnvelope.IsValidToSend)
                _logger.Warn("Invalid Envelope or Message");
            else
            {
                byte[] bytesToSend = outgoingEnvelope.Message.Encode();

                _logger.DebugFormat($"Send out: {Encoding.ASCII.GetString(bytesToSend)} to {outgoingEnvelope.EndPoint}");

                try
                {
                    _myUdpClient.Send(bytesToSend, bytesToSend.Length, outgoingEnvelope.EndPoint);
                    _loggerDeep.Debug("Send complete");
                }
                catch (Exception err)
                {
                    error = new Error()
                    {
                        Text = $"Cannnot send a {outgoingEnvelope.Message.GetType().Name} to {outgoingEnvelope.EndPoint}: {err.Message}"
                    };
                    _logger.Warn(error.Text);
                }
            }
            return error;
        }

        public Error JoinMulticastGroup(IPAddress groupAddres)
        {
            Error error = null;
            try
            {
                _myUdpClient.JoinMulticastGroup(groupAddres);
            }
            catch (Exception err)
            {
                error = new Error() { Text = $"Cannot join multicast group: {err}" };
            }
            return error;
        }

        public Error DropMulticastGroup(IPAddress groupAddres)
        {
            Error error = null;
            try
            {
                _myUdpClient.DropMulticastGroup(groupAddres);
            }
            catch (Exception err)
            {
                error = new Error() { Text = $"Cannot join multicast group: {err}" };
            }
            return error;
        }
        #endregion

        #region Private Methods
        private void Receive()
        {
            while (_keepGoing)
            {
                var env = ReceiveOne();
                if (env != null)
                    EnvelopeHandler?.Invoke(env);
            }
        }

        private Envelope ReceiveOne()
        {
            Envelope result = null;

            IPEndPoint senderEndPoint;
            byte[] receivedBytes = ReceiveBytes(Timeout, out senderEndPoint);
            if (receivedBytes != null && receivedBytes.Length > 0)
            {
                var message = Message.Decode(receivedBytes);
                if (message != null)
                {
                    result = new Envelope(message, senderEndPoint);
                    _logger.Debug(
                        $"Just received message, Nr={result.Message.MsgId?.ToString() ?? "null"}, " +
                        $"Conv={result.Message.ConvId?.ToString() ?? "null"}, " +
                        $"Type={result.Message.GetType().Name}, " +
                        $"From={result.IpEndPoint?.ToString() ?? "null"}");
                }
                else
                {
                    _logger.Error($"Cannot decode message received from {senderEndPoint}");
                    _logger.Error($"Message={Encoding.ASCII.GetString(receivedBytes)}");
                }
            }

            return result;
        }

        private byte[] ReceiveBytes(int timeout, out IPEndPoint ep)
        {
            ep = null;
            if (_myUdpClient == null) return null;

            byte[] receivedBytes = null;
            _myUdpClient.Client.ReceiveTimeout = timeout;
            ep = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                _loggerDeep.Debug("Try receive bytes from anywhere");
                receivedBytes = _myUdpClient.Receive(ref ep);
                _loggerDeep.Debug("Back from receive");

                if (_logger.IsDebugEnabled)
                {
                    var tmp = Encoding.ASCII.GetString(receivedBytes);
                    _logger.Debug($"Incoming message={tmp}");
                }
            }
            catch (SocketException err)
            {
                if (err.SocketErrorCode != SocketError.TimedOut && err.SocketErrorCode != SocketError.Interrupted)
                    _logger.Warn(err.Message);
            }
            catch (Exception err)
            {
                _logger.Warn(err.Message);
            }
            return receivedBytes;
        }

        private void ValidPorts()
        {
            if ((MinPort != 0 && (MinPort < IPEndPoint.MinPort || MinPort > IPEndPoint.MaxPort)) ||
                (MaxPort != 0 && (MaxPort < IPEndPoint.MinPort || MaxPort > IPEndPoint.MaxPort)))
                throw new ApplicationException("Invalid port specifications");
        }

        private int FindAvailablePort(int minPort, int maxPort)
        {
            int availablePort = -1;

            _logger.DebugFormat("Find a free port between {0} and {1}", minPort, maxPort);
            for (int possiblePort = minPort; possiblePort <= maxPort; possiblePort++)
            {
                if (!IsUsed(possiblePort))
                {
                    availablePort = possiblePort;
                    break;
                }
            }
            _logger.DebugFormat("Available Port = {0}", availablePort);
            return availablePort;
        }

        private bool IsUsed(int port)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var endPoints = properties.GetActiveUdpListeners();
            return endPoints.Any(ep => ep.Port == port);
        }
        #endregion

    }
}
