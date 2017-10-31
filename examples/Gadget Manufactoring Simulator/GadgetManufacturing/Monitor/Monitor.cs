using System;
using System.Threading;
using AppShared;
using CommSub;
using log4net;
using SharedObjects;

namespace Monitor
{
    public class Monitor : AppProcess
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Monitor));
        private static readonly ILog LoggerDeep = LogManager.GetLogger(typeof(Monitor) + "_Deep");
        private MonitorState _state;

        public RuntimeOptions MonitorOptions => Options as RuntimeOptions;

        public override ProcessType ProcessType => ProcessType.Monitor;

        #region Constructors, Initializers, and Destructors
        public override void Start()
        {
            Logger.Debug("Entering Start");

            RegistryEndPoint = new PublicEndPoint(MonitorOptions.RegistryHostAndPort);

            ConversationFactory conversationFactory = new ConversationFactory()
            {
                DefaultMaxRetries = MonitorOptions.Retries,
                DefaultTimeout = MonitorOptions.Timeout
            };

            _state = new MonitorState()
            {
                ProcessType = ProcessType,
                Options = MonitorOptions
            };

            SetupCommSubsystem(_state, conversationFactory);

            base.Start();

            Logger.Debug("Leaving Start");
        }

        public override void Stop()
        {
            Logger.Debug("Entering Stop");

            if (_state.State == CommProcessState.PossibleState.Terminating ||
                _state.State == CommProcessState.PossibleState.NotInitialized) return;

            _state.State = CommProcessState.PossibleState.Terminating;

            base.Stop();

            _state.State = CommProcessState.PossibleState.NotInitialized;

            Logger.DebugFormat($"Leaving Stop, with Status={MyCommSubsystem.ProcessState.State.ToString()}");
        }
        #endregion

        #region Private and Protected Methods
        protected override void Process(object state)
        {
            _state.State = CommProcessState.PossibleState.Registering;

            DoRegistration();

            try
            {
                Logger.Debug("Before main MyProcess loop");
                while (KeepGoing && _state.State == CommProcessState.PossibleState.Running)
                {
                    // TODO: Make decision about getting gadget from Gadget Assemblers

                    Thread.Sleep(1000);
                    LoggerDeep.DebugFormat("At bottom of Main MyProcess Loop");
                }
                Logger.DebugFormat("Out of Main MyProcess Loop, with KeepGoing={0}, Status={1}", KeepGoing, _state.State);

                Stop();
            }
            catch (Exception err)
            {
                Logger.Error(err.ToString());
            }

        }

        #endregion

    }
}
