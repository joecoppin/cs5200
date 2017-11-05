using System.Threading;
using CommSubsystem.Conversations;
using log4net;
using Messages;

namespace SharedAppLayer.Conversations
{
    public class DiscoveryOfServerInitiator : InitiatorConversation
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DiscoveryOfServerInitiator));

        private bool _waiting;
        private Timer _timeoutTimer;

        public static int SuccesCount { get; private set; }

        public static int FailureCount { get; private set; }

        protected override void ExecuteDetails()
        {
            Error = CommFacility.Send(FirstEnvelope);
            if (Error != null)
            {
                FailureCount++;
                return;
            }

            _timeoutTimer = new Timer(TimeoutHandler, null, Timeout, System.Threading.Timeout.Infinite);
            _waiting = true;
            var isSuccess = false;

            while (_waiting)
            {
                var incomingEnvelope = ConvQueue.Dequeue(Timeout);
                if (!IsEnvelopeValid(incomingEnvelope, typeof(ServerAliveMessage))) continue;

                var serverInfo = new ServerInfo()
                                        {
                                            ProcessId = incomingEnvelope.Message.MsgId.Pid,
                                            EndPoint = incomingEnvelope.EndPoint
                                        };
                SuccessAction?.Invoke(serverInfo);
                isSuccess = true;
            }

            if (isSuccess)
                SuccesCount++;
            else
                FailureCount++;

            Logger.Debug($"Exiting ExecuteDetals, with isSuccess={isSuccess}");
        }

        private void TimeoutHandler(object state)
        {
            _timeoutTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            _timeoutTimer = null;
            _waiting = false;
        }
    }
}
