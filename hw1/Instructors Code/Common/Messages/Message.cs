using System;
using System.Net;
using log4net;

namespace Common.Messages
{
    public abstract class Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Message));

        public enum PossibleMessageTypes
        {
            Unknown = 0,
            NewGame = 1,
            GameDef = 2,
            Guess = 3,
            Answer = 4,
            GetHint = 5,
            Hint = 6,
            Exit = 7,
            Ack = 8,
            Error = 9,
            Heartbeat = 10,
            GetStatus = 11,
            Status = 12
        };
        private PossibleMessageTypes _messageType;

        protected Message(PossibleMessageTypes type)
        {
            _messageType = type;
        }

        public PossibleMessageTypes MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        public short GameId { get; set; }

        /// <summary>
        /// End Pointer of message's target, if this message was sent on a socket.
        /// </summary>
        public IPEndPoint TargetEndPoint { get; set; } = null;

        /// <summary>
        /// End Pointer of message's send, if this message was receive on a socket.  If was created locally, then it is null.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; set; } = null;

        public abstract byte[] Encode();

        protected abstract void Decode(ByteList bytes);

        public abstract bool IsValid { get; }

        public static Message Create(byte[] receivedBytes)
        {
            Message result = null;
            if (receivedBytes!=null && receivedBytes.Length>=2)
            {
                ByteList bytes = new ByteList(receivedBytes);
                PossibleMessageTypes messageType = (PossibleMessageTypes) bytes.GetShort();

                switch (messageType)
                {
                    case PossibleMessageTypes.NewGame:
                        result = new NewGameMessage();
                        break;
                    case PossibleMessageTypes.GameDef:
                        result = new GameDef();
                        break;
                    case PossibleMessageTypes.Guess:
                        result = new GuessMessage();
                        break;
                    case PossibleMessageTypes.Answer:
                        result = new AnswerMessage();
                        break;
                    case PossibleMessageTypes.GetHint:
                        result = new GetHintMessage();
                        break;
                    case PossibleMessageTypes.Hint:
                        result = new HintMessage();
                        break;
                    case PossibleMessageTypes.Error:
                        result = new ErrorMessage();
                        break;
                    case PossibleMessageTypes.Exit:
                        result = new ExitMessage();
                        break;
                    case PossibleMessageTypes.Heartbeat:
                        result = new HeartbeatMessage();
                        break;
                    case PossibleMessageTypes.Ack:
                        result = new AckMessage();
                        break;
                    case PossibleMessageTypes.GetStatus:
                        result = new GetStatusMessage();
                        break;
                    case PossibleMessageTypes.Status:
                        result = new StatusMessage();
                        break;
                }

                try
                {
                    result?.Decode(bytes);
                }
                catch (Exception err)
                {
                    Log.ErrorFormat("Cannot decode a {0} message", result?.GetType().Name);
                    Log.ErrorFormat("Inner Error: {0}", err.Message);
                }
            }
            return result;
        }

        public override string ToString()
        {
            string result = $"Type={_messageType}, {Parameters}";
            if (SenderEndPoint != null)
                result += " from " + SenderEndPoint;
            return result;
        }

        protected abstract string Parameters { get; }

    }
}
