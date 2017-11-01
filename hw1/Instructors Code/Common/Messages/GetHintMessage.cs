using log4net;

namespace Common.Messages
{
    public class GetHintMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GetHintMessage));

        public GetHintMessage() : base(PossibleMessageTypes.GetHint) { }

        public GetHintMessage(short gameId) : base(PossibleMessageTypes.GetHint)
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
            Log.Debug("Decode an GetHintMessage");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);
        }

        public override bool IsValid => (GameId > 0);

        protected override string Parameters => $"GameId={GameId}";

    }
}
