using System;
using System.Collections.Generic;
using CommSubsystem.Conversations;

namespace CommSubsystem
{
    public abstract class ConversationFactory
    {
        private readonly Dictionary<Type, Type> _incomingMessageMappings = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> _outgoingMessageMappings = new Dictionary<Type, Type>();

        public CommFacility ManagingCommFacility { get; set; }
        public int DefaultMaxRetries { get; set; }
        public int DefaultTimeout { get; set; }

        public bool IncomingMessageCanStartConversation(Type messageType)
        {
            return _incomingMessageMappings.ContainsKey(messageType);
        }

        public bool OutgoingMessageCanStartConversation(Type messageType)
        {
            return _outgoingMessageMappings.ContainsKey(messageType);
        }

        public virtual Conversation Create(Envelope firstEnvelope,
            Conversation.ActionHandler preInitializationHandler = null,
            Conversation.ActionHandler preExecutionHandler = null,
            Conversation.ActionHandler successHandler = null,
            Conversation.ActionHandler failureHandler = null,
            Conversation.ActionHandler postExecutionHandler = null)
        {
            Type convType = null;
            var messageType = firstEnvelope?.Message?.GetType();
            if (messageType == null) return null;

            if (IncomingMessageCanStartConversation(messageType))
                convType = _incomingMessageMappings[messageType];
            else if (OutgoingMessageCanStartConversation(messageType))
                convType = _outgoingMessageMappings[messageType];

            if (convType == null) return null;

            var conversation = Activator.CreateInstance(convType) as Conversation;
            if (conversation == null) return null;

            conversation.CommFacility = ManagingCommFacility;
            conversation.FirstEnvelope = firstEnvelope;
            conversation.PreInitializationAction = preInitializationHandler;
            conversation.PreExecuteAction = preExecutionHandler;
            conversation.SuccessAction = successHandler;
            conversation.FailureAction = failureHandler;
            conversation.PostExecuteAction = postExecutionHandler;
            conversation.MaxRetries = DefaultMaxRetries;
            conversation.Timeout = DefaultTimeout;

            return conversation;
        }

        public abstract void Initialize();

        #region Methods for building up mappings of messageType to converstionType
        protected void AddIncomingMapping(  Type messageType, Type conversationType)
        {
            if (messageType == null || conversationType == null || _incomingMessageMappings.ContainsKey(messageType))
                return;

            _incomingMessageMappings.Add(messageType, conversationType);
        }

        protected void AddOutgoingMapping(  Type messageType, Type conversationType)
        {
            if (messageType == null || conversationType == null || _outgoingMessageMappings.ContainsKey(messageType))
                return;

            _outgoingMessageMappings.Add(messageType, conversationType);
        }

        #endregion
    }
}
