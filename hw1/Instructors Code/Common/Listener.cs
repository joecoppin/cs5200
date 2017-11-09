using System;
using Common.Messages;
using log4net;

namespace Common
{
    public class Listener : BackgroundThread
    {
        #region Private data members
        private static readonly ILog Log = LogManager.GetLogger(typeof(Listener));

        private readonly int _timeout;
        private readonly Communicator _comm;

        #endregion

        #region Constructors and destruction
        public Listener() { }

        public Listener(Communicator comm, int timeout)
        {
            _comm = comm;
            _timeout = timeout;
        }
        #endregion

        #region Public Methods

        public override string ThreadName()
        {
            return "Listener";
        }

        #endregion

        #region Private methods
        protected override void Process()
        {
            while (KeepGoing)
            {
                try
                {
                    Message m = _comm.Receive(_timeout);
                    if (m != null && !Suspended)
                    {
                        Log.Debug("Recieved message - " + m);
                        _comm.Enqueue(m);
                    }
                }

                catch (Exception err)
                {
                    Log.Fatal("Caught an excepted exception, the Listener will die - ", err);
                    KeepGoing = false;
                }
            }

            Log.Info("Leaving Process method");
        }
        #endregion
    }
}
