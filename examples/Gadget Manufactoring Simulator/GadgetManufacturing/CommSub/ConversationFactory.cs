using System;
using System.Collections.Generic;

namespace CommSub
{
    public abstract class ConversationFactory
    {
        private readonly Dictionary<Type, Type> _typeMappings = new Dictionary<Type, Type>();

        public CommSubsystem ManagingSubsystem { get; set; }
        public int DefaultMaxRetries { get; set; }
        public int DefaultTimeout { get; set; }

        public Conversation.ActionHandler PreExecuteAction { get; set; }
        public Conversation.ActionHandler PostExecuteAction { get; set; }

        public bool IncomingMessageCanStartConversation(Type messageType)
        {
            return _typeMappings.ContainsKey(messageType);
        }

        public virtual Conversation CreateFromMessage(Envelope envelope)
        {
            Conversation conversation = null;
            Type messageType = envelope?.Message?.GetType();

            if (messageType != null && _typeMappings.ContainsKey(messageType))
                conversation = CreateResponderConversation(_typeMappings[messageType], envelope);

            return conversation;
        }

        public virtual T CreateFromConversationType<T>() where T : Conversation, new()
        {
            T conversation = new T()
            {
                CommSubsystem = ManagingSubsystem,
                MaxRetries = DefaultMaxRetries,
                Timeout = DefaultTimeout,
                PreExecuteAction = PreExecuteAction,
                PostExecuteAction = PostExecuteAction
            };
            return conversation;
        }

        public abstract void Initialize();

        protected void Add(Type messageType, Type conversationType)
        {
            if (messageType != null && conversationType != null && !_typeMappings.ContainsKey(messageType))
                _typeMappings.Add(messageType, conversationType);
        }

        protected virtual ResponderConversation CreateResponderConversation(Type conversationType, Envelope envelope = null)
        {
            ResponderConversation conversation = null;
            if (conversationType != null)
            {
                conversation = Activator.CreateInstance(conversationType) as ResponderConversation;
                if (conversation != null)
                {
                    conversation.CommSubsystem = ManagingSubsystem;
                    conversation.MaxRetries = DefaultMaxRetries;
                    conversation.Timeout = DefaultTimeout;
                    conversation.IncomingEnvelope = envelope;
                    conversation.PreExecuteAction = PreExecuteAction;
                    conversation.PostExecuteAction = PostExecuteAction;
                }
            }
            return conversation;
        }
    }
}
