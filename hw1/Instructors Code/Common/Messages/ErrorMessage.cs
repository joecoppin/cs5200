using log4net;

namespace Common.Messages
{
    public class ErrorMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ErrorMessage));

        public ErrorMessage() : base(PossibleMessageTypes.Error) { }

        public ErrorMessage(short gameId, string errorText) : base(PossibleMessageTypes.Error)
        {
            GameId = gameId;
            ErrorText = errorText;
        }

        public string ErrorText { get; set; }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short) MessageType);
            list.Add(GameId);
            list.Add(ErrorText);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode an ErrorMessage");
            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);

            if (!bytes.IsMore) return;
            ErrorText = bytes.GetString();
            Log.DebugFormat("Decoded ErrorText = {0}", ErrorText);
        }

        public override bool IsValid => (GameId > 0);

        protected override string Parameters => $"GameId={GameId}, ErrorText={ErrorText}";
    }
}
