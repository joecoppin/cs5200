using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

using Messages;
using SharedObjects;
using log4net;

namespace CommSub
{
    public delegate void IncomingEnvelopeHandler(Envelope env);
    /// <summary>
    /// This class provides an abstraction for sending and receiving envelopes from a datagram socket (UdpClient).  Its objects are active,
    /// in that the receive functionality runs on its own thread.  When a message is received, it is placed on a conversation queue or
    /// used to start a new conversation.
    /// </summary>
    public class UdpCommunicator
    {
        #region Private Data Members
        private ILog _logger;
        private ILog _loggerDeep;
        private UdpClient _myUdpClient;
        private Thread _receiveThread;
        private bool _started;
        private static readonly object StartStopLock = new object();
        #endregion

        #region Public Properties
        public int MinPort { get; set; }
        public int MaxPort { get; set; }
        public int Timeout { get; set; }
        public int Port => ((IPEndPoint) _myUdpClient?.Client.LocalEndPoint)?.Port ?? 0;
        public string LoggerPrefix { get; set; }
        public IncomingEnvelopeHandler EnvelopeHandler { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method starts up the UdpCommunicator by a) creating a UdpClient that is bound to a free port between the specified MinPort and MaxPort and b)
        /// starting a thread that looks for incoming messsages.  When that thread gets an incoming message, it will package it up in an envelope and callback
        /// to the specified EnvelopeHandler, which should be a method in the CommSubsystem
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
                if (_started) Stop();

                int portToTry = FindAvailablePort(MinPort, MaxPort);
                if (portToTry > 0)
                {
                    try
                    {
                        IPEndPoint localEp = new IPEndPoint(IPAddress.Any, portToTry);
                        _myUdpClient = new UdpClient(localEp);
                        _started = true;
                    }
                    catch (SocketException)
                    {
                    }
                }

                if (!_started)
                    throw new ApplicationException($"Cannot bind the socket to a port {portToTry}");
                else
                {
                    _receiveThread = new Thread(Receive);
                    _receiveThread.Start();
                }
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
                _started = false;
                _receiveThread?.Join(Timeout * 2);
                _receiveThread = null;

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

                _logger.DebugFormat("Send out: {0} to {1}", Encoding.ASCII.GetString(bytesToSend), outgoingEnvelope.EndPoint);

                try
                {
                    _myUdpClient.Send(bytesToSend, bytesToSend.Length, outgoingEnvelope.EndPoint.IpEndPoint);
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
                error = new Error() {Text = $"Cannot join multicast group: {err}"};
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
            while (_started)
            {
                Envelope env = ReceiveOne();
                if (env != null)
                    EnvelopeHandler?.Invoke(env);
            }
        }

        private Envelope ReceiveOne()
        {
            Envelope result = null;

            IPEndPoint ep;
            byte[] receivedBytes = ReceiveBytes(Timeout, out ep);
            if (receivedBytes != null && receivedBytes.Length > 0)
            {
                PublicEndPoint sendersEndPoint = new PublicEndPoint() { IpEndPoint = ep };
                ControlMessage message = ControlMessage.Decode(receivedBytes);
                if (message != null)
                {
                    result = new Envelope(message, sendersEndPoint);
                    _logger.DebugFormat("Just received message, Nr={0}, Conv={1}, Type={2}, From={3}",
                                            result.Message.MsgId?.ToString() ?? "null",
                                            result.Message.ConvId?.ToString() ?? "null",
                                            result.Message.GetType().Name,
                                            result.IpEndPoint?.ToString() ?? "null");
                }
                else
                {
                    _logger.ErrorFormat("Cannot decode message received from {0}", sendersEndPoint);
                    _logger.ErrorFormat("Message={0}", Encoding.ASCII.GetString(receivedBytes));
                }
            }

            return result;
        }

        private byte[] ReceiveBytes(int timeout, out IPEndPoint ep)
        {
            byte[] receivedBytes = null;
            ep = null;
            if (_myUdpClient != null)
            {
                _myUdpClient.Client.ReceiveTimeout = timeout;
                ep = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    _loggerDeep.Debug("Try receive bytes from anywhere");
                    receivedBytes = _myUdpClient.Receive(ref ep);
                    _loggerDeep.Debug("Back from receive");

                    if (_logger.IsDebugEnabled)
                    {
                        if (receivedBytes != null)
                        {
                            string tmp = Encoding.ASCII.GetString(receivedBytes);
                            _logger.DebugFormat("Incoming message={0}", tmp);
                        }
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
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] endPoints = properties.GetActiveUdpListeners();
            return endPoints.Any(ep => ep.Port == port);
        }
        #endregion

    }
}
