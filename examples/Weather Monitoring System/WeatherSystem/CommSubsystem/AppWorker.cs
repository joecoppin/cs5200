using System;
using System.Threading.Tasks;
using log4net;
using SharedObjects;

namespace CommSubsystem
{
    public abstract class AppWorker
    {
        #region Private Data Members
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AppWorker));

        protected bool KeepGoing;
        #endregion

        #region Public Properties
        public event StateChange.Handler Startup;
        public event StateChange.Handler Update;
        public event StateChange.Handler Error;
        public event StateChange.Handler Shutdown;

        public RuntimeOptions Options { get; set; }
        public CommFacility CommFacility { get; protected set; }

        public enum PossibleStatus
        {
            None,
            Working,
            Stopped
        };
        public PossibleStatus Status { get; set; }
        #endregion

        #region Constructors, Initializers, Destructors
        public virtual void Start()
        {
            // Setup up Process Id
            LocalProcessInfo.Instance.ProcessId = Options.ProcessId;
            LocalProcessInfo.Instance.StartTime = DateTime.Now;

            // Create and setup the conversation factory
            var conversationFactory = CreateConversationFactory();      // Abstract method implement by specific type of work
            conversationFactory.ManagingCommFacility = CommFacility;
            conversationFactory.DefaultTimeout = Options.Timeout;
            conversationFactory.DefaultMaxRetries = Options.MaxRetries;
            conversationFactory.Initialize();

            // Create and setup the comm facility
            CommFacility = new CommFacility(this, conversationFactory);
            CommFacility.Initialize();
            CommFacility.Start();

            // Start running
            Task.Factory.StartNew(Run);
            RaiseStartupEvent();
        }

        public void Stop()
        {
            Logger.DebugFormat("Entering Stop");

            KeepGoing = false;

            CommFacility.Stop();

            RaiseShutdownEvent();

            Logger.DebugFormat("Leaving Stop");
        }
        #endregion

        #region Public Methods for Raising Events
        public void RaiseStartupEvent()
        {
            if (Startup != null)
            {
                Logger.Debug("Raise Startup event");
                Startup(new StateChange() { Type = StateChange.ChangeType.Startup, Subject = this });
                Logger.Debug("Back from raising Startup event");
            }
            else
                Logger.Debug("Nothing is registered for the Startup event");
        }

        public void RaiseUpdateEvent(object context)
        {
            if (Update != null)
            {
                Logger.Debug("Raise Update event");
                Update(new StateChange() { Type = StateChange.ChangeType.Update, Subject = this, Context = context });
                Logger.Debug("Back from raising update event");
            }
            else
                Logger.Debug("Nothing is registered for the Update event");
        }

        public void RaiseErrorEvent(object context)
        {
            if (Error != null)
            {
                Logger.Debug("Raise Error event");
                Error(new StateChange() { Type = StateChange.ChangeType.Error, Subject = this, Context = context });
                Logger.Debug("Back from raising Error event");
            }
            else
                Logger.Debug("Nothing is registered for the Error event");
        }

        public void RaiseShutdownEvent()
        {
            if (Shutdown != null)
            {
                Logger.Debug("Raise Shutdown event");
                Shutdown(new StateChange() { Type = StateChange.ChangeType.Shutdown, Subject = this });
                Logger.Debug("Back from raising Shutdown event");
            }
            else
                Logger.Debug("Nothing is registered for the Shutdown event");
        }
        #endregion

        protected abstract void Run();

        protected abstract ConversationFactory CreateConversationFactory();
    }
}
