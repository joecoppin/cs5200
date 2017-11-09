using System;
using System.Threading;

using Common.Messages;
using log4net;
namespace Common
{
    public class HeartbeatGenerator : BackgroundThread
    {
        public delegate void HeartbeatSender(Communicator comm);

        #region Private data members
        private static readonly ILog Log = LogManager.GetLogger(typeof(HeartbeatGenerator));

        private readonly int _heartbeatInterval;
        private readonly HeartbeatSender _heartbeatSender;
        private readonly Communicator _comm;

        #endregion

        #region Constructors and destruction
        public HeartbeatGenerator(Communicator comm, int intervalWaitTime, HeartbeatSender senderMethod)
        {
            _heartbeatInterval = intervalWaitTime;
            _heartbeatSender = senderMethod;
            _comm = comm;
        }
        #endregion

        #region Public Methods

        public override string ThreadName()
        {
            return "HeartbeatGenerator";
        }

        #endregion

        #region Private methods
        protected override void Process()
        {
            Log.Debug("Entering Process");

            while (KeepGoing)
            {
                try
                {
                    Thread.Sleep(_heartbeatInterval);
                    _heartbeatSender(_comm);
                }

                catch (Exception err)
                {
                    Log.Fatal("Caught an excepted exception, the Listener will die - ", err);
                    KeepGoing = false;
                }
            }

            Log.Debug("Leaving Process");
        }
        #endregion
    }
}
