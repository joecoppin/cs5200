using log4net;

namespace Common.Messages
{
    public class AnswerMessage : Message
    {
        #region Private Data Members
        private static readonly ILog Log = LogManager.GetLogger(typeof(AnswerMessage));
        #endregion

        public AnswerMessage() : base(PossibleMessageTypes.Answer) { }

        public AnswerMessage(short gameId, bool result, short score, string hint) : base(PossibleMessageTypes.Answer)
        {
            GameId = gameId;
            Result = result;
            Score = score;
            Hint = hint;
        }

        public bool Result { get; set; }

        public short Score { get; set; }

        public string Hint { get; set; }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short)MessageType);
            list.Add(GameId);
            list.Add(Result);
            list.Add(Score);
            list.Add(Hint);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode a AnswerMessage");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);

            if (!bytes.IsMore) return;
            Result = (bytes.GetByte() == 1);
            Log.DebugFormat("Decoded Result = {0}", Result);

            if (!bytes.IsMore) return;
            Score = bytes.GetShort();
            Log.DebugFormat("Decoded Score = {0}", Score);

            if (!bytes.IsMore) return;
            Hint = bytes.GetString();
            Log.DebugFormat("Decoded Hint = {0}", Hint);
        }

        public override bool IsValid => (GameId > 0);

        protected override string Parameters => $"GameId={GameId}, Result={Result}, Score={Score}, Hint={Hint}";

    }
}
