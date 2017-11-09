using log4net;
using System;
using System.Collections.Generic;

namespace Common.Messages
{
    public class StatusMessage : Message
    {
        #region Private Data Members
        private static readonly ILog Log = LogManager.GetLogger(typeof(StatusMessage));

        #endregion

        public StatusMessage() : base(PossibleMessageTypes.Status) { }

        public int ActiveGameCount { get; set; }

        public List<PlayerInfo> Students { get; } = new List<PlayerInfo>();

        public override byte[] Encode()
        {
            ByteList bytes = new ByteList();

            bytes.Add((short) MessageType);
            bytes.Add(ActiveGameCount);
            bytes.Add(Students.Count);
            foreach (PlayerInfo s in Students)
            {
                s.Encode(bytes);
            }

            return bytes.ToBytes();
        }

        protected override void Decode(ByteList bytes)
        {
            Log.Debug("Decode a AnswerMessage");

            if (bytes == null || !bytes.IsMore) return;
            ActiveGameCount = bytes.GetInt();
            Log.Debug($"Decoded ActiveGameCount = {ActiveGameCount}");

            int studentCount = bytes.GetInt();
            Log.Debug($"Decoded StudentCount = {studentCount}");

            for (var i = 0; i < studentCount; i++)
            {
                PlayerInfo s = new PlayerInfo();
                s.Decode(bytes);
                Log.Debug($"Decoded Student: {s}");
                Students.Add(s);
            }
        }

        public override bool IsValid => true;

        protected override string Parameters => $"ActiveGameCount={ActiveGameCount}";

        public class PlayerInfo
        {
            public int Id { get; set; }
            public string Alias { get; set; }
            public int GameCount { get; set; }
            public int GuessCount { get; set; }
            public int HintCount { get; set; }
            public int ExitCount { get; set; }
            public int HeartbeatCount { get; set; }
            public int HighScore { get; set; }
            public DateTime LastMessage { get; set; }

            public void Encode(ByteList bytes)
            {
                bytes.Add(Id);
                bytes.Add(Alias);
                bytes.Add(GameCount);
                bytes.Add(GuessCount);
                bytes.Add(HintCount);
                bytes.Add(ExitCount);
                bytes.Add(HeartbeatCount);
                bytes.Add(HighScore);
                bytes.Add(LastMessage.ToBinary());
            }

            public void Decode(ByteList bytes)
            {
                Id = bytes.GetInt();
                Alias = bytes.GetString();
                GameCount = bytes.GetInt();
                GuessCount = bytes.GetInt();
                HintCount = bytes.GetInt();
                ExitCount = bytes.GetInt();
                HeartbeatCount = bytes.GetInt();
                HighScore = bytes.GetInt();
                LastMessage = DateTime.FromBinary(bytes.GetLong());

                Log.Debug($"Decoded Player = {ToString()}");
            }

            public override string ToString()
            {
                return $"{Alias},{GameCount},{GuessCount},{HintCount},{ExitCount},{HeartbeatCount},{HighScore},{LastMessage}";
            }
        }
    }
}
