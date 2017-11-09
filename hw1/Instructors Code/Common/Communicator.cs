﻿using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using log4net;
using Common.Messages;

namespace Common
{
    /// <summary>
    /// Summary description for Communicator.
    /// </summary>
    public class Communicator
    {
        #region Private and Protected Data Members
        private static readonly ILog Log = LogManager.GetLogger(typeof(Communicator));

        private int _localPort;
        private IPEndPoint _localEndPoint;
        private UdpClient _udpClient;

        private MessageQueue _queue;
        private AutoResetEvent _queueWaitHandle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Communicator()
        {
            Initialize();
        }

        /// <summary>
        /// Primary Constructor
        /// </summary>
        /// <param name="localPort">If non-zero, the communicator will attempt to use this port</param>
        public Communicator(int localPort)
        {
            _localPort = localPort;
            Initialize();
        }

        public void Initialize()
        {
            Log.Debug("Initializing communicator");

            _localEndPoint = new IPEndPoint(IPAddress.Any, _localPort);
            _udpClient = new UdpClient(_localEndPoint);
            Log.Debug("Creating UdpClient with end point " + _localEndPoint);

            _localEndPoint = _udpClient.Client.LocalEndPoint as IPEndPoint;
            if (_localEndPoint != null)
            {
                _localPort = _localEndPoint.Port;
                Log.Info("Created Communicator's UdpClient, bound to " + _localEndPoint);

                _queue = new MessageQueue();
                _queueWaitHandle = new AutoResetEvent(false);

                Log.Debug("Done initializing communicator");
            }
        }

        #endregion

        #region Public Properties and Methods

        public bool CommunicationsEnabled => _udpClient != null;

        public Int32 LocalPort
        {
            get { return _localPort; }
            set { _localPort = value; }
        }

        public IPEndPoint LocalEndPoint => _localEndPoint;

        public void Enqueue(Message m)
        {
            if (m != null)
            {
                Log.Debug("Enqueue message = " + m);
                _queue.Enqueue(m);
                _queueWaitHandle.Set();
            }
        }

        public bool MessageAvailable(int timeout)
        {
            bool result = false;
            if (_queue.Count > 0)
                result = true;
            else if (timeout > 0)
                result = _queueWaitHandle.WaitOne(timeout, true);
            return result;
        }

        public Message Dequeue()
        {
            Message result = null;

            if (_queue.Count > 0)
                result = _queue.Dequeue();

            if (result != null)
                Log.Debug("Dequeue message = " + result);
            return result;
        }

        public Message Receive(int timeout)
        {
            Log.Debug("Entering Receive");

            Message result = null;
            try
            {
                // Wait for some data to become available
                while (CommunicationsEnabled && _udpClient?.Available <= 0 && timeout > 0)
                {
                    Thread.Sleep(10);
                    timeout -= 10;
                }

                // If there is data receive and communications are enabled, then read that data
                if (CommunicationsEnabled && _udpClient?.Available > 0)
                {
                    Log.InfoFormat(@"Packet available");
                    var ep = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes = _udpClient?.Receive(ref ep);
                    Log.Debug($"Bytes received: {FormatBytesForDisplay(receiveBytes)}");
                    result = Message.Create(receiveBytes);
                    if (result != null)
                    {
                        result.SenderEndPoint = ep;
                        Log.InfoFormat($"Received {result} from {ep}");
                    }
                    else
                    {
                        Log.Warn(@"Data received, but could not be decoded");
                    }
                }
            }
            catch (SocketException err)
            {
                if (err.SocketErrorCode != SocketError.TimedOut && err.SocketErrorCode != SocketError.ConnectionReset)
                    Log.ErrorFormat($"Socket error: {err.SocketErrorCode}, {err.Message}");
            }
            catch (Exception err)
            {
                Log.ErrorFormat($"Unexpected expection while receiving datagram: {err} ");
            }
            Log.Debug("Leaving Receive");
            return result;
        }

        public bool Send(Message msg, IPEndPoint targetEndPoint)
        {
            msg.TargetEndPoint = targetEndPoint;
            return Send(msg);
        }

        public bool Send(Message msg)
        {
            Log.Debug("Entering Send");

            bool result = false;

            if (CommunicationsEnabled && msg?.TargetEndPoint != null)
            {
                try
                {
                    Log.Debug($"Send {msg} to {msg.TargetEndPoint}");
                    byte[] buffer = msg.Encode();
                    Log.Debug($"Bytes sent: {FormatBytesForDisplay(buffer)}");
                    int count = _udpClient.Send(buffer, buffer.Length, msg.TargetEndPoint);
                    result = (count == buffer.Length);
                    Log.Info($"Sent {msg} to {msg.TargetEndPoint}, result={result}");
                }
                catch (Exception err)
                {
                    Log.Error("Unexpected exception while sending datagram - ", err);
                }
            }

            Log.Debug("Leaving Send, result = " + result);
            return result;
        }

        public void Close()
        {
            _udpClient?.Close();
        }

        #endregion

        private string FormatBytesForDisplay(byte[] bytes)
        {
            return bytes.Aggregate(string.Empty, (current, b) => current + (b.ToString("X") + " "));
        }
    }
}
