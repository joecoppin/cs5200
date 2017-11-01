using log4net;

namespace Common.Messages
{
    public class GuessMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GuessMessage));

        public GuessMessage() : base(PossibleMessageTypes.Guess) { }

        public GuessMessage(short gameId, string word) : base(PossibleMessageTypes.Guess)
        {
            GameId = gameId;
            Word = word;
        }

        public string Word { get; set; }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short) MessageType);
            list.Add(GameId);
            list.Add(Word);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode an GuessMessage");

            if (bytes == null || !bytes.IsMore) return;
            GameId = bytes.GetShort();
            Log.DebugFormat("Decoded GameId = {0}", GameId);

            if (!bytes.IsMore) return;
            Log.Debug("Try to get the FirstName");
            if (bytes.PeekShort() < 0 && bytes.PeekShort() > 40)
                Log.WarnFormat("The length of the encoded Word, {0}, is probably not correct.  Check how you are encoding the string's length", bytes.PeekShort());
            Word = bytes.GetString();
            Log.DebugFormat("Decoded Word = {0}", Word);
        }

        public override bool IsValid => !string.IsNullOrWhiteSpace(Word);

        protected override string Parameters => $"GameId={GameId}, Word={Word}";
    }
}
