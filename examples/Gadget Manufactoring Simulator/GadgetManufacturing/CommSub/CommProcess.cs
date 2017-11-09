using SharedObjects;
using Utils;

using log4net;

namespace CommSub
{
    /// <summary>
    /// This class can be a base class for any process that needs to use the communication subsystem.  Such a
    /// process is an active object that run with behavior that needs to run on a background thread.  So, this
    /// class inherits from BackgroundThread.
    /// 
    /// A specialization of this class will need to implement the active behavior by overriding the Process method.
    /// Before exit, the Process method should call Stop() to stop communication subsystem.
    /// 
    /// This class contains a communicationsubsystem, runtime options, the Proxy's end point, an error history, and
    /// some convenient methods for working with these things.
    /// 
    /// It also has a Shutdown event that could be used to wire up the hanlding of a shutdown message.  A
    /// shutdown would raise the event.  The GUI and other active objects could be listening for that event
    /// and stop one it occurs.
    /// </summary>
    public abstract class CommProcess : BackgroundThread
    {
        #region Private Data Members
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CommProcess));

        protected const int MainLoopSleep = 200;
        protected object MyLock = new object();
        #endregion

        #region Public Properties
        public RuntimeOptions Options { get; set; }
        public CommSubsystem MyCommSubsystem { get; protected set; }
        public string BestLocalEndPoint => MyCommSubsystem?.BestLocalEndPoint;
        #endregion

        #region Constructors, Initializers, Destructors
        protected virtual void SetupCommSubsystem(CommProcessState processState, ConversationFactory conversationFactory)
        {
            MyCommSubsystem = new CommSubsystem(processState, conversationFactory);
            MyCommSubsystem.Initialize();
            MyCommSubsystem.Start();
        }

        public override void Stop()
        {
            Logger.DebugFormat("Entering Stop");

            base.Stop();

            MyCommSubsystem.Stop();

            RaiseShutdownEvent();

            Logger.DebugFormat("Leaving Stop");
        }
        #endregion

        #region Public Events and Methods for Raising Events
        public event StateChange.Handler Shutdown;

        public void RaiseShutdownEvent()
        {
            Logger.Debug("Enter RaiseShutdownEvent");
            if (Shutdown != null)
            {
                Logger.Debug("Raise Shutdown event");
                Shutdown(new StateChange() { Type = StateChange.ChangeType.Shutdown, Subject = this });
                Logger.Debug("Back from raising Shutdown event");
            }
            else
                Logger.Debug("Nothing is registered for the shutdown");
            Logger.Debug("Leave RaiseShutdownEvent");
        }

        #endregion

    }

}
