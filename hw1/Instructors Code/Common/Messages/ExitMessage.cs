using log4net;

namespace Common.Messages
{
    public class ExitMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExitMessage));

        public ExitMessage() : base(PossibleMessageTypes.Exit) { }
        
        public ExitMessage(short gameId) : base(PossibleMessageTypes.Exit)
        {
            GameId = gameId;
        }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short)MessageType);
            list.Add(GameId);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode an ErrorMessage");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);
        }

        public override bool IsValid => (GameId > 0);

        protected override string Parameters => $"GameId={GameId}";
    }
}
