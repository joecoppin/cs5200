using log4net;

namespace Common.Messages
{
    public class GetStatusMessage : Message
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GetHintMessage));

        public GetStatusMessage() : base(PossibleMessageTypes.GetStatus) { }

        public override byte[] Encode()
        {
            ByteList list = new ByteList();

            list.Add((short)MessageType);

            return list.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode an GetStatus");
        }

        public override bool IsValid => (true);

        protected override string Parameters => $"";
    }
}
