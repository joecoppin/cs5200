using System;
using CommSub;
using Messages;
using SharedObjects;
using log4net;

namespace AppShared.Conversations.InitiatorConversations
{
    public class Registration : InitiatorRRConversation
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Registration));

        protected override ControlMessage CreateFirstMessage()
        {
            return new Register() { ProcessType = CommSubsystem.ProcessState.ProcessType };
        }

        protected override Type ExceptedReplyType => typeof(ProcessInfo);

        protected override void ProcessValidResponse(Envelope env)
        {
            Logger.Debug("Got a ProcessInfo message");
            ProcessInfo msg = env.Message as ProcessInfo;
            LocalProcessInfo.Instance.ProcessId = msg.ProcessId;
            CommSubsystem.ProcessState.State = CommProcessState.PossibleState.Running;

            Logger.DebugFormat("ProcessId={0}, State={1}", LocalProcessInfo.Instance.ProcessId, CommSubsystem.ProcessState.State);
        }

    }
}
