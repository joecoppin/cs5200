using System;
using System.Threading;
using log4net;

namespace Common
{
    public abstract class BackgroundThread
    {
        #region Private data members
        private static readonly ILog Log = LogManager.GetLogger(typeof(BackgroundThread));

        protected Thread ParentThread;
        protected Timer ParentTimer;
        protected Thread MyThread;
        private bool _suspended;

        #endregion

        #region Constructors and destruction

        protected BackgroundThread()
        {
            ParentThread = Thread.CurrentThread;
            ParentTimer = new Timer(ParentChecker, null, 1000, 1000);

        }
        #endregion

        #region Public Methods
        public virtual void Start()
        {
            try
            {
                KeepGoing = true;
                Log.Info("Starting " + ThreadName());
                MyThread = new Thread(Process) {Name = ThreadName()};
                MyThread.Start();
            }
            catch (Exception err)
            {
                Log.Fatal("Aborted exception caught", err);
            }
        }

        public virtual void Stop()
        {
            if (MyThread != null)
            {
                Log.InfoFormat("Stopping {0}", ThreadName());
                KeepGoing = false;
                // MyThread.Join();
                MyThread = null;
                Log.InfoFormat("{0} Stopped", ThreadName());
            }
        }

        public abstract string ThreadName();

        public bool IsRunning => (MyThread != null && MyThread.IsAlive);

        public virtual string Status => (_suspended) ? "Suspended" : "Running";

        public virtual void Suspend()
        {
            if (Suspended == false)
                Suspended = true;
        }

        public virtual void Resume()
        {
            if (Suspended)
                Suspended = false;
        }

        public virtual bool Suspended
        {
            get { return _suspended; }
            set
            {
                _suspended = value;
                Log.Info(ThreadName() + " - " + Status);
            }
        }

        protected bool KeepGoing { get; set; }

        #endregion

        #region Private methods
        protected abstract void Process();

        protected void ParentChecker(object state)
        {
            if (KeepGoing && ParentThread != null && !ParentThread.IsAlive)
                KeepGoing = false;
        }
        #endregion


    }
}
