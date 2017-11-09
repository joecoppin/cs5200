using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;

namespace WordGuessServer
{
    public class Player
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Player));
        private static readonly Dictionary<string, Player> PlayerList = new Dictionary<string, Player>();
        private static string _playersFileName;
        private static bool _changed;

        private static readonly object ListLock = new object();

        private string _aNumber;
        private string _lastName;
        private string _firstName;
        private string _alias;
        private int _gameCount;
        private int _guessCount;
        private int _hintCount;
        private int _exitCount;
        private int _heartbeatCount;
        private int _highScore;
        private DateTime _lastMessage;
        private bool? _communicatedFromAws;


        public string ANumber
        {
            get { return _aNumber; }
            set
            {
                _changed = (_aNumber != value);
                _aNumber = value;
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _changed = (_lastName != value);
                _lastName = value;
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _changed = (_firstName != value);
                _firstName = value;
            }
        }

        public string Alias
        {
            get { return _alias; }
            set
            {
                _changed = (_alias != value);
                _alias = value;
            }
        }

        public int GameCount => _gameCount;
        public int GuessCount => _guessCount;
        public int HintCount => _hintCount;
        public int ExitCount => _exitCount;
        public int HeartbeatCount => _heartbeatCount;

        public void IncGameCount()
        {
            _gameCount++;
            _changed = true;
        }

        public void IncGuessCount()
        {
            _guessCount++;
            _changed = true;
        }

        public void IncHintCount()
        {
            _hintCount++;
            _changed = true;
        }

        public void IncExitCount()
        {
            _exitCount++;
            _changed = true;
        }

        public void IncHeartbeatCount()
        {
            _heartbeatCount++;
            _changed = true;
        }

        public int HighScore
        {
            get { return _highScore; }
            set
            {
                _changed = (_highScore != value);
                _highScore = value;
            }
        }
        public DateTime LastMessage
        {
            get { return _lastMessage; }
            set
            {
                _changed = (_lastMessage != value);
                _lastMessage = value;
            }
        }


        public bool? CommunicatedFromAws
        {
            get { return _communicatedFromAws; }
            set
            {
                _changed = (_communicatedFromAws != value);
                _communicatedFromAws = value;
            }
        }

        private Player(string aNumber, string lastName, string firstName, string alias)
        {
            ANumber = aNumber;
            LastName = lastName;
            FirstName = firstName;
            Alias = alias;
        }

        public override string ToString()
        {
            return $"{ANumber},{LastName},{FirstName},{Alias},{GameCount},{GuessCount},{HintCount},{ExitCount},{HeartbeatCount},{HighScore},{LastMessage},{CommunicatedFromAws}";
        }

        public static Player Create(string aNumber, string lastName, string firstName, string alias)
        {
            Log.Debug($"Create student with {aNumber}, {lastName}, {firstName}, {alias}");
            Player result = null;
            lock (ListLock)
            {
                if (PlayerList.ContainsKey(aNumber))
                {
                    result = PlayerList[aNumber];
                    result.FirstName = firstName;
                    result.LastName = lastName;
                    result.Alias = alias;
                }
                else if (!string.IsNullOrWhiteSpace(aNumber) &&
                         !string.IsNullOrWhiteSpace(lastName) &&
                         !string.IsNullOrWhiteSpace(firstName))
                {
                    if (string.IsNullOrWhiteSpace(alias))
                        alias = firstName;

                    result = new Player(aNumber, lastName, firstName, alias);
                    PlayerList.Add(aNumber, result);
                    _changed = true;
                }
            }
            return result;
        }

        public static void LoadFromLine(string line)
        {
            Log.Debug("Load Student for File");
            if (string.IsNullOrWhiteSpace(line)) return;

            var fields = line.Split(',');

            Log.Debug("Number of fields");
            if (fields.Length != 11 && fields.Length != 12) return;

            var student = Create(fields[0], fields[1], fields[2], fields[3]);
            if (student != null)
            {
                student._gameCount = ConvertToInt(fields[4]);
                student._guessCount = ConvertToInt(fields[5]);
                student._hintCount = ConvertToInt(fields[6]);
                student._exitCount = ConvertToInt(fields[7]);
                student._heartbeatCount = ConvertToInt(fields[8]);
                student._highScore = ConvertToInt(fields[9]);
                student._lastMessage = ConvertToDateTime(fields[10]);
                if (fields.Length == 12)
                    student._communicatedFromAws = ConvertToBool(fields[11]);
                _changed = true;
            }
        }

        public static bool LoadFile(string fileName)
        {
            Log.DebugFormat("load Student Data from {0}", fileName);

            bool result = false;

            _playersFileName = fileName;
            if (File.Exists(fileName))
            {
                StreamReader reader = new StreamReader(fileName);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    LoadFromLine(line);
                }
                reader.Close();
                result = true;
            }
            return result;
        }

        public static void SaveFile()
        {
            if (!_changed) return;

            Log.Debug($"Save Student Data to {_playersFileName}");
            StreamWriter writer = new StreamWriter(_playersFileName);

            lock (ListLock)
            {
                Dictionary<string, Player>.Enumerator enumerator = PlayerList.GetEnumerator();
                while (enumerator.MoveNext())
                    writer.WriteLine(enumerator.Current.Value.ToString());
            }
            writer.Close();
            _changed = false;
        }

        public static List<Player> KnownPlayers
        {
            get
            {
                List<Player> result;
                lock (ListLock)
                {
                    result = PlayerList.Values.ToList();
                }
                return result;
            }   
        }

        private static int ConvertToInt(string value)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                // ignored
            }
            return result;
        }

        private static DateTime ConvertToDateTime(string value)
        {
            DateTime result = DateTime.MinValue;
            try
            {
                result = Convert.ToDateTime(value);
            }
            catch (Exception)
            {
                // ignored
            }
            return result;
        }

        private static bool? ConvertToBool(string value)
        {
            bool? result = null;
            try
            {
                result = Convert.ToBoolean(value);
            }
            catch (Exception)
            {
                // ignored
            }
            return result;
        }
        
    }
}
