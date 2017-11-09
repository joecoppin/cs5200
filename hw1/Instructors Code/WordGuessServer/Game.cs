using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Common;
using Common.Messages;
using log4net;

namespace WordGuessServer
{
    public class Game
    {
        #region Private data members
        private static readonly ILog Log = LogManager.GetLogger(typeof(Game));

        private static readonly Random RandomGenerator = new Random();
        private static short _nextId;
        private static Dictionary<short, Game> _gameSet = new Dictionary<short, Game>();
        private static readonly ServerSettings Settings = new ServerSettings();

        private readonly string _word;
        private readonly string _wordDefinition;
        private string _disclosedWord;
        private short _correctLetters;
        private readonly DateTime _startTime;
        private DateTime _lastMessageTime;
        private DateTime _lastAckTime;
        private DateTime? _endTime;
        private int _guessCount;
        private int _hintCount;
        private short _score = -1;
        private bool _lookingForAck;

        private static readonly object GameSetLock = new object();

        #endregion

        public Game()
        {
            Log.Debug("Creating a new game");

            Id = GetNextId();
            _startTime = DateTime.Now;
            _lastAckTime = _startTime;
            _word = WordDictionary.GetRandomWord();
            _wordDefinition = WordDictionary.GetDefinition(_word);
            _disclosedWord = "".PadRight(_word.Length, '_');
            
            lock (GameSetLock)
            {
                _gameSet.Add(Id, this);
            }

            Log.Debug("Game created using " + _word + ", " + _disclosedWord + ", " + _wordDefinition);
        }

        public IPEndPoint PlayerEndPoint { get; set; }

        public short Id { get; set; }

        public Player Student { get; set; }

        public string Word => _word;

        public string WordDefinition => _wordDefinition;

        public string DisclosedWord => _disclosedWord;

        public short CorrectLetters => _correctLetters;

        public short Score => _score;

        public bool HasEnded => (_endTime != null);

        public DateTime LastMessageTime
        {
            get { return _lastMessageTime; }
            set
            {
                _lastMessageTime = value;
                Student.LastMessage = _lastMessageTime;
            }
        }

        public string Hint
        {
            get
            {
                Log.Debug("Compute next hint");
                List<int> blankPlaces = new List<int>(Word.Length);
                for (int i = 0; i < _disclosedWord.Length; i++)
                    if (_disclosedWord[i] == '_')
                        blankPlaces.Add(i);

                if (blankPlaces.Count > 0)
                {
                    int placeToFillIn = blankPlaces[0];
                    if (blankPlaces.Count > 1)
                        placeToFillIn = blankPlaces[RandomGenerator.Next(0, blankPlaces.Count-1)];

                    StringBuilder builder = new StringBuilder(_disclosedWord) {[placeToFillIn] = _word[placeToFillIn]};
                    _disclosedWord = builder.ToString();
                }
                _hintCount++;
                Log.Debug("Hint #" + _hintCount + "=" + _disclosedWord);
                return _disclosedWord;
            }
        }

        public int Guess(string guess)
        {
            Log.Debug("Check a guess, "+guess);
            _correctLetters = 0;
            _guessCount++;

            if (!String.IsNullOrEmpty(guess))
            {
                for (int i = 0; i < guess.Length && i < _word.Length; i++)
                {
                    if (guess[i] == _word[i])
                        _correctLetters++;
                }
            }
            if (_correctLetters == _word.Length)
            {
                Log.Debug("Correct Guess!");
                _endTime = DateTime.Now;
                ComputeScore();
            }

            Log.DebugFormat("correctLetters={0}", _correctLetters);
            return _correctLetters;
        }

        public void GotAck()
        {
            Log.Debug("Entering GotAck");
            if (_lookingForAck)
            {
                Log.DebugFormat("Got heartbeat for game {0}", Id);
                _lastAckTime = DateTime.Now;
                _lookingForAck = false;
            }
        }

        public void Exit()
        {
            Log.Debug("Exit Game");
            _endTime = DateTime.Now;
            _score=0;
        }

        public static Game FindGame(short gameId)
        {
            Game result = null;
            lock (GameSetLock)
            {
                if (_gameSet.ContainsKey(gameId))
                    result = _gameSet[gameId]; 
            }
            return result;
        }

        public static int ActiveGameCount
        {
            get
            {
                int count;
                lock (GameSetLock)
                {
                    count = _gameSet.Count;
                }
                return count;
            }
        }

        public static void CleanupGames()
        {
            lock (GameSetLock)
            {
                Dictionary<short, Game> newGameSet = new Dictionary<short, Game>();
                Log.Info("Cleanup Games");
                Dictionary<short, Game>.Enumerator enumerator = _gameSet.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    bool shouldDelete = false;
                    Game game = enumerator.Current.Value;
                    TimeSpan inactiveTimeSpan = DateTime.Now.Subtract(game._lastAckTime);
                    Log.DebugFormat("Game {0}, Inactive for {1} ms", game.Id, inactiveTimeSpan.TotalMilliseconds);

                    if (inactiveTimeSpan.TotalMilliseconds > Settings.CleanupTime)
                    {
                        shouldDelete = true;
                        Log.DebugFormat("Mark game {0} for deletion", game.Id);
                    }

                    if (!shouldDelete)
                        newGameSet.Add(game.Id, game);
                }
                _gameSet = newGameSet;
            }

        }

        public void SendOutHeartbeat(Communicator comm)
        {
            Log.DebugFormat("Sending heartbeat to game {0} at {1}", Id, PlayerEndPoint);
            HeartbeatMessage msg = new HeartbeatMessage(Id);
            if (comm.Send(msg, PlayerEndPoint))
                _lookingForAck = true;
            else
                _lastAckTime = DateTime.MinValue;
        }

        public static void SendOutHeartbeats(Communicator comm)
        {
            lock (GameSetLock)
            {
                Dictionary<short, Game>.Enumerator enumerator = _gameSet.GetEnumerator();
                while (enumerator.MoveNext())
                    if (!enumerator.Current.Value.HasEnded)
                        enumerator.Current.Value.SendOutHeartbeat(comm);
            }
        }

        private void ComputeScore()
        {
            Log.Debug("Get score");
            if (_endTime != null && _score < 0)
            {
                Log.Debug("Compute score");
                TimeSpan tmp = ((DateTime)_endTime).Subtract(_startTime);
                _score = Convert.ToInt16(Math.Max(0,
                    ((_word.Length - _hintCount) * Settings.CharValue) -
                    (Convert.ToInt32(tmp.TotalSeconds) * Settings.CostPerSecond) -
                    ((_guessCount - 1) * Settings.CostPerGuess)));
            }
            Log.Debug("Score=" + _score);
        }

        private static short GetNextId()
        {
            if (_nextId == Int16.MaxValue)
                _nextId = 0;
            _nextId++;
            return _nextId;
        }
    }
}
