using CommSub;
using Messages;
using SharedObjects;

using log4net;

namespace Registry.Conversations.ResponderConversations
{
    public class Registration : ResponderConversation
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Registration));

        private RegistryState MyProcessState => CommSubsystem.ProcessState as RegistryState;

        protected override bool UseConverstationQueue => false;

        protected override void ExecuteDetails(object context)
        {
            Logger.Debug("Entering ExecuteDetails");

            Register registerRequest = IncomingEnvelope.Message as Register;
            if (registerRequest == null)
                Error = new Error()
                {
                    Text = "Incoming messages does not contain a Register message"
                };
            else if (registerRequest.ProcessType == ProcessType.Unknown || registerRequest.ProcessType == ProcessType.Register)
                Error = new Error()
                {
                    Text = $"Incoming Register message has an invalid process type of {registerRequest.ProcessType}"
                };
            else
            {
                short newProcessId = MyProcessState.RegisterProcess(registerRequest.ProcessType, IncomingEnvelope.EndPoint);

                if (newProcessId == 0)
                    Error = new Error()
                    {
                        Text = "Cannot register process"
                    };
                else
                {
                    ControlMessage msg = new ProcessInfo() {ProcessId = newProcessId};
                    msg.SetMessageAndConversationIds(MessageId.Create(), ConvId);
                    Envelope env = new Envelope() { Message = msg, EndPoint = RemotEndPoint };
                    Error = CommSubsystem.Send(env);
                }
            }

            Logger.Debug("Leaving ExecuteDetails");
        }
    }
}
