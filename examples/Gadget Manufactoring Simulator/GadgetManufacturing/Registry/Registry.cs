using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommSub;

using log4net;
using Registry.Conversations.InitialConversations;
using SharedObjects;

namespace Registry
{
    public class Registry : CommProcess
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Registry));
        private static readonly ILog LoggerDeep = LogManager.GetLogger(typeof(Registry) + "_Deep");
        private RegistryState _state;

        public RuntimeOptions RegistryOptions => Options as RuntimeOptions;

        #region Constructors, Initializers, and Destructors
        public override void Start()
        {
            Logger.Debug("Entering Start");

            LocalProcessInfo.Instance.ProcessId = 1;

            ConversationFactory conversationFactory = new ConversationFactory()
            {
                DefaultMaxRetries = RegistryOptions.Retries,
                DefaultTimeout = RegistryOptions.Timeout
            };

            _state = new RegistryState()
            {
                Options = RegistryOptions
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

            Parallel.ForEach(_state.RegisteredProcesses, entry =>
            {
                Shutdown conv = MyCommSubsystem.CreateFromConversationType<Shutdown>();
                conv.RemotEndPoint = entry.EndPoint;
                conv.Execute();
            });

            base.Stop();

            _state.State = CommProcessState.PossibleState.NotInitialized;

            Logger.DebugFormat($"Leaving Stop, with Status={MyCommSubsystem.ProcessState.State.ToString()}");
        }
        #endregion

        #region Public methods

        public List<RegistryState.RegistryEntry> RegisteredProcesses => _state.RegisteredProcesses;
        public int RegisteredProcessCount => _state.RegisteredProcessCount;

        public void StartShutdown()
        {
            foreach (RegistryState.RegistryEntry entry in _state.RegisteredProcesses)
            {
                Conversations.InitialConversations.Shutdown conv =
                    MyCommSubsystem.CreateFromConversationType<Shutdown>();
                conv.RemotEndPoint = entry.EndPoint;
                conv.Launch(null);
            }
        }

        #endregion

        #region Private and Protected Methods
        protected override void Process(object state)
        {
            _state.State = CommProcessState.PossibleState.Running;

            try
            {
                Logger.Debug("Before main MyProcess loop");
                while (KeepGoing && _state.State == CommProcessState.PossibleState.Running)
                {
                    QueryAlive();
                    CleanupDeadProcesses();

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

        private void CleanupDeadProcesses()
        {
            // TODO            
        }


        private void QueryAlive()
        {
            // TODO
        }

        #endregion

    }
}
