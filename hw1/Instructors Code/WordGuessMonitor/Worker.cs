using System;
using log4net;
using Common;
using Common.Messages;

namespace WordGuessMonitor
{
    public class Worker : BackgroundThread
    {
        public delegate void MessageHandler(Message msg);

        #region Private data members
        private static readonly ILog Log = LogManager.GetLogger(typeof(Worker));

        private readonly Communicator _comm;
        private readonly int _timeout;
        #endregion

        public event MessageHandler GotStatus;

        #region Constructors and destruction
        public Worker(Communicator comm, int timeout)
        {
            _comm = comm;
            _timeout = timeout;
        }
        #endregion

        #region Public Methods

        public override string ThreadName()
        {
            return "Worker";
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Main message process loop!
        /// 
        /// This routine continues until the work is stopped.  It first looks for an available message
        /// in the queue.  If it finds one, it executes that request.
        /// </summary>
        protected override void Process()
        {
            while (KeepGoing)
            {
                if (_comm.MessageAvailable(_timeout))
                {
                    var msg = _comm.Dequeue();
                    if (msg != null && msg.IsValid)
                    {
                        Log.Info("Processing " + msg);
                        ProcessMessage(msg);
                    }
                }
            }
        }

        private void ProcessMessage(Message msg)
        {
            Log.Debug($"Enter ProcessMessage, with a {msg.MessageType}, {msg.GetType()}");

            Message reply = null;

            try
            {
                switch (msg.MessageType)
                {
                    case Message.PossibleMessageTypes.Status:
                        Log.Debug("Process Status Message");
                        GotStatus?.Invoke(msg);
                        break;
                    default:
                        reply = new ErrorMessage(msg.GameId, $"Monitor does not accept a message of type {msg.MessageType}");
                        break;
                }
            }
            catch (Exception err)
            {
                reply = new ErrorMessage(msg.GameId, err.Message);
            }

            if (reply != null)
                _comm.Send(reply, msg.SenderEndPoint);

            Log.Debug("Leaving ProcessMessage");
        }

        #endregion

    }
}
