using log4net;

namespace Common.Messages
{
    public class HeartbeatMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HeartbeatMessage));

        public HeartbeatMessage() : base(PossibleMessageTypes.Heartbeat)
        {
        }

        public HeartbeatMessage(ByteList bytes) : base(PossibleMessageTypes.Heartbeat)
        {

        }

        public HeartbeatMessage(short gameId) : base(PossibleMessageTypes.Heartbeat)
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
            Log.Debug("Decode an HeartbeatMessage");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);
        }

        public override bool IsValid => (GameId > 0);

        protected override string Parameters => $"GameId={GameId}";
    }
}
