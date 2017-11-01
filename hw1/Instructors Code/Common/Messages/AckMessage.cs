using log4net;

namespace Common.Messages
{
    public class AckMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AckMessage));

        public AckMessage() : base(PossibleMessageTypes.Ack) { }

        public AckMessage(short gameId) : base(PossibleMessageTypes.Ack)
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
            Log.Debug("Decode a AckMessage");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);
        }

        public override bool IsValid => (GameId > 0);

        protected override string Parameters => $"GameId={GameId}";
    }
}
