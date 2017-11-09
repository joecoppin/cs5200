using System;
using System.Threading;
using AppShared;
using CommSub;
using SharedObjects;

using log4net;

namespace AppSharedTesting
{
    public class MockAppProcess : AppProcess
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MockAppProcess));
        private MockAppProcessState _state;

        public RuntimeOptions MockProcessOptions => Options as RuntimeOptions;

        public CommProcessState.PossibleState State => _state.State;
        public override ProcessType ProcessType => SharedObjects.ProcessType.GadgetWarehouse;

        #region Constructors, Initializers, and Destructors
        public override void Start()
        {
            Logger.Debug("Entering Start");

            RegistryEndPoint = new PublicEndPoint(MockProcessOptions.RegistryHostAndPort);

            ConversationFactory conversationFactory = new ConversationFactory()
            {
                DefaultMaxRetries = MockProcessOptions.Retries,
                DefaultTimeout = MockProcessOptions.Timeout
            };

            _state = new MockAppProcessState()
            {
                ProcessType = ProcessType,
                Options = MockProcessOptions
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

        protected override void Process(object state)
        {
            while (KeepGoing && _state.State == CommProcessState.PossibleState.Running)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
