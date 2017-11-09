using log4net;

namespace Common.Messages
{
    public class GameDef : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameDef));

        public GameDef() : base(PossibleMessageTypes.GameDef) { }

        public GameDef(short gameId, string hint, string definition) : base(PossibleMessageTypes.GameDef)
        {
            GameId = gameId;
            Hint = hint;
            Definition = definition;
        }

        public string Hint { get; set; }

        public string Definition { get; set; }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short)MessageType);
            list.Add(GameId);
            list.Add(Hint);
            list.Add(Definition);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode an GameDef");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);

            if (!bytes.IsMore) return;
            Hint = bytes.GetString();
            Log.DebugFormat("Decoded Hint = {0}", Hint);

            if (!bytes.IsMore) return;
            Definition = bytes.GetString();
            Log.DebugFormat("Decoded Definition = {0}", Definition);
        }

        public override bool IsValid =>
                    GameId > 0 &&
                    !string.IsNullOrWhiteSpace(Hint) &&
                    !string.IsNullOrWhiteSpace(Definition);

        protected override string Parameters => $"GameId={GameId}, Hint={Hint}, LastName={Definition}";

    }
}
