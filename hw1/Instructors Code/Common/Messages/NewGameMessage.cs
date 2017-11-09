using System.Windows.Forms;
using log4net;

namespace Common.Messages
{
    public class NewGameMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewGameMessage));

        public NewGameMessage() : base(PossibleMessageTypes.NewGame) { }

        public NewGameMessage(string aNumber, string lastName, string firstName, string alias) : base(PossibleMessageTypes.NewGame)
        {
            ANumber = aNumber;
            LastName = lastName;
            FirstName = firstName;
            Alias = alias;
        }

        public string ANumber { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
        public string Alias { get; set; }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short)MessageType);
            list.Add(ANumber);
            list.Add(LastName);
            list.Add(FirstName);
            list.Add(Alias);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode a NewGameMessage");

            if (bytes == null || !bytes.IsMore) return;
            Log.Debug("Try to get the A#");
            if (bytes.PeekShort()<0 && bytes.PeekShort()>24)
                Log.Warn($"The length of the encoded A#, {bytes.PeekShort()}, is probably not correct.  Check how you are encoding the string's length");

            ANumber = bytes.GetString();
            Log.Debug($"Decoded A# = {ANumber}"); if (string.IsNullOrWhiteSpace(ANumber) || !ANumber.StartsWith("A"))
                Log.Warn($"The ANumber doesn't start with an A, and therefore may not be encoded correctly");


            if (!bytes.IsMore) return;
            Log.Debug("Try to get the LastName");
            if (bytes.PeekShort() < 0 && bytes.PeekShort() > 48)
                Log.Warn($"The length of the encoded LastName, {bytes.PeekShort()}, is probably not correct.  Check how you are encoding the string's length");
            LastName = bytes.GetString();
            Log.Debug($"Decoded LastName = {LastName}");

            if (!bytes.IsMore) return;
            Log.Debug("Try to get the FirstName");
            if (bytes.PeekShort() < 0 && bytes.PeekShort() > 48)
                Log.Warn($"The length of the encoded FirstName, {bytes.PeekShort()}, is probably not correct.  Check how you are encoding the string's length");
            FirstName = bytes.GetString();
            Log.Debug($"Decoded FirstName = {FirstName}");

            if (!bytes.IsMore) return;
            Log.Debug("Try to get the Alias");
            if (bytes.PeekShort() < 0 && bytes.PeekShort() > 48)
                Log.Warn($"The length of the encoded Alias, {bytes.PeekShort()}, is probably not correct.  Check how you are encoding the string's length");
            Alias = bytes.GetString();
            Log.Debug($"Decoded Alias = {Alias}");
        }

        public override bool IsValid =>
                    !string.IsNullOrWhiteSpace(ANumber) &&
                    !string.IsNullOrWhiteSpace(LastName) &&
                    !string.IsNullOrWhiteSpace(FirstName);

        protected override string Parameters => $"ANumber={ANumber}, LastName={LastName}, FirstName={FirstName}";

    }
}
