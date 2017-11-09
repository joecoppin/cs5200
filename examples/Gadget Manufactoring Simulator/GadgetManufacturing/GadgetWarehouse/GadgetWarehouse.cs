using System;
using System.Threading;
using CommSub;
using log4net;

using AppShared;
using SharedObjects;

namespace GadgetWarehouse
{
    public class GadgetWarehouse : AppProcess
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GadgetWarehouse));
        private static readonly ILog LoggerDeep = LogManager.GetLogger(typeof(GadgetWarehouse) + "_Deep");
        private GadgetWarehouseState _state;

        public RuntimeOptions GadgetWarehouseOptions => Options as RuntimeOptions;

        public override ProcessType ProcessType => ProcessType.GadgetWarehouse;

        #region Constructors, Initializers, and Destructors
        public override void Start()
        {
            Logger.Debug("Entering Start");

            RegistryEndPoint = new PublicEndPoint(GadgetWarehouseOptions.RegistryHostAndPort);

            ConversationFactory conversationFactory = new ConversationFactory()
            {
                DefaultMaxRetries = GadgetWarehouseOptions.Retries,
                DefaultTimeout = GadgetWarehouseOptions.Timeout
            };

            _state = new GadgetWarehouseState()
            {
                ProcessType = ProcessType,
                Options = GadgetWarehouseOptions
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
