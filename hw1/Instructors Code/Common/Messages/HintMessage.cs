using log4net;

namespace Common.Messages
{
    public class HintMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HintMessage));

        public HintMessage() : base(PossibleMessageTypes.Hint) { }

        public HintMessage(short gameId, string hint) : base(PossibleMessageTypes.Hint)
        {
            GameId = gameId;
            Hint = hint;
        }

        public string Hint { get; set; }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short)MessageType);
            list.Add(GameId);
            list.Add(Hint);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode an HintMessage");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);

            if (!bytes.IsMore) return;
            Hint = bytes.GetString();
            Log.DebugFormat("Decoded Hint = {0}", Hint);
        }

        public override bool IsValid => !string.IsNullOrWhiteSpace(Hint);

        protected override string Parameters => $"GameId={GameId}, Word={Hint}";
    }
}
