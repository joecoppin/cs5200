using SharedObjects;

namespace CommSub
{
    public class CommProcessState
    {
        public ProcessType ProcessType { get; set; }
        public enum PossibleState { NotInitialized, Initialized, Registering, Running, ReceivedShutdown, Terminating }
        public PossibleState State { get; set; } = PossibleState.NotInitialized;
        public RuntimeOptions Options { get; set; }
        public PublicEndPoint CategoryMulticast { get; set; }
        public PublicEndPoint GmsMulticast { get; set; }

        public event StateChange.Handler Change;
        public void RaiseChangedEvent(StateChange change)
        {
            Change?.Invoke(change);
        }
    }
}
