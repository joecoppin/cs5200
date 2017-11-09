using System;
using System.Net;

using log4net;
using Common;
using Common.Messages;

namespace WordGuessServer
{
    /// <summary>
    /// Summary description for Doer.
    /// </summary>
    public class Worker : BackgroundThread
    {
        #region Private data members
        private static readonly ILog Log = LogManager.GetLogger(typeof(Worker));

        private readonly Communicator _comm;
        private readonly int _timeout;
        private readonly string _studentResultsFile;
        #endregion

        #region Constructors and destruction
        public Worker(Communicator comm, int timeout, string studentResultsFile)
        {
            _comm = comm;
            _timeout = timeout;
            _studentResultsFile = studentResultsFile;
        }
        #endregion

        #region Public Methods

        public override string ThreadName()
        {
            return "Worker";
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Main message process loop!
        /// 
        /// This routine continues until the work is stopped.  It first looks for an available message
        /// in the queue.  If it finds one, it executes that request.
        /// </summary>
        protected override void Process()
        {
            Log.Debug("Start Processing");
            Player.LoadFile(_studentResultsFile);
            while (KeepGoing)
            {
                if (_comm.MessageAvailable(_timeout))
                {
                    var msg = _comm.Dequeue();
                    if (msg != null && msg.IsValid)
                    {
                        Log.InfoFormat("Processing {0}", msg);
                        ProcessMessage(msg);
                    }
                }
                else
                    Player.SaveFile();
            }
            Player.SaveFile();
            Log.Debug("End Processing");
        }

        private void ProcessMessage(Message msg)
        {
            Log.Debug("Entering ProcessMessage");
            if (msg == null) return;

            Message reply = null;
            try
            {
                switch (msg.MessageType)
                {
                    case Message.PossibleMessageTypes.NewGame:
                        reply = ProcessNewGameMessage(msg as NewGameMessage);
                        break;
                    case Message.PossibleMessageTypes.Guess:
                        reply = ProcessGuessMessage(msg as GuessMessage);
                        break;
                    case Message.PossibleMessageTypes.GetHint:
                        reply = ProcessGetHintMessage(msg as GetHintMessage);
                        break;
                    case Message.PossibleMessageTypes.Exit:
                        reply = ProcessExitMessage(msg as ExitMessage);
                        break;
                    case Message.PossibleMessageTypes.Ack:
                        ProcessAckMessage(msg as AckMessage);
                        break;
                    case Message.PossibleMessageTypes.GetStatus:
                        reply = ProcessGetStatusMessage(msg as GetStatusMessage);
                        break;
                    default:
                        reply = new ErrorMessage(msg.GameId, $"Server does not accept a message of type {msg.MessageType}");
                        break;
                }
            }
            catch(Exception err)
            {
                reply = new ErrorMessage(msg.GameId, err.Message);
            }

            if (reply!=null)
                _comm.Send(reply, msg.SenderEndPoint);

            Log.Debug("Leaving ProcessMessage");
        }

        private Message ProcessNewGameMessage(NewGameMessage newGameMessage)
        {
            Message reply;
            if (newGameMessage.IsValid)
            {
                Log.Debug("Process a newgame message");
                var student = Player.Create(newGameMessage.ANumber, newGameMessage.LastName, newGameMessage.FirstName, newGameMessage.Alias);
                if (IsAws(newGameMessage.SenderEndPoint.Address))
                    student.CommunicatedFromAws = true;
                else if (student.CommunicatedFromAws == null)
                    student.CommunicatedFromAws = false;

                var game = new Game() { Student = student, PlayerEndPoint = newGameMessage.SenderEndPoint};

                Log.Debug("Game created");
                reply = new GameDef(game.Id, game.DisclosedWord, game.WordDefinition);
                student.IncGameCount();
                game.LastMessageTime = DateTime.Now;
            }
            else
            {
                Log.InfoFormat("Invalid NewGame Message from {0}", newGameMessage.SenderEndPoint);
                reply = new ErrorMessage(newGameMessage.GameId, "Invalid NewGame message");
            }

            return reply;
        }

        private Message ProcessGuessMessage(GuessMessage guessMessage)
        {
            Message reply;
            if (guessMessage.IsValid)
            {
                var game = Game.FindGame(guessMessage.GameId);
                if (game == null)
                {
                    Log.InfoFormat("Game Id {0} in Guess Message from {1}", guessMessage.GameId, guessMessage.SenderEndPoint);
                    reply = new ErrorMessage(guessMessage.GameId, $"Invalid game id = {guessMessage.GameId}");
                }
                else
                {
                    game.Guess(guessMessage.Word);
                    if (game.HasEnded)
                    {
                        Log.DebugFormat("Send back a winning answer");
                        reply = new AnswerMessage(game.Id, game.HasEnded, game.Score, game.Word);
                        game.Student.HighScore = Math.Max(game.Student.HighScore, game.Score);
                    }
                    else
                    {
                        Log.DebugFormat("Send back a failed answer");
                        reply = new AnswerMessage(game.Id, game.HasEnded, game.CorrectLetters, game.Hint);
                    }

                    game.Student.IncGuessCount();
                    game.LastMessageTime = DateTime.Now;
                }
            }
            else
            {
                Log.InfoFormat("Invalid Guess Message from {0}", guessMessage.SenderEndPoint);
                reply = new ErrorMessage(guessMessage.GameId, "Invalid guess message");
            }

            return reply;
        }

        private Message ProcessGetHintMessage(GetHintMessage getHintMessage)
        {
            Message reply;
            if (getHintMessage.IsValid)
            {
                var game = Game.FindGame(getHintMessage.GameId);
                if (game == null)
                {
                    Log.InfoFormat("Invalid Game Id {0} from {1}", getHintMessage.GameId, getHintMessage.SenderEndPoint);
                    reply = new ErrorMessage(getHintMessage.GameId, $"Invalid game id = {getHintMessage.GameId}");
                }
                else
                {
                    reply = new HintMessage(game.Id, game.Hint);
                    game.Student.IncHintCount();
                    game.LastMessageTime = DateTime.Now;
                }
            }
            else
            {
                Log.InfoFormat("Invalid getHit message from {0}", getHintMessage.SenderEndPoint);
                reply = new ErrorMessage(getHintMessage.GameId, @"Invalid getHint message");
            }
            return reply;
        }

        private Message ProcessExitMessage(ExitMessage exitMessage)
        {
            Message reply;
            if (exitMessage.IsValid)
            {
                var game = Game.FindGame(exitMessage.GameId);
                if (game == null)
                {
                    Log.InfoFormat("Invalid Game Id {0} from {1}", exitMessage.GameId, exitMessage.SenderEndPoint);
                    reply = new ErrorMessage(exitMessage.GameId, $"Invalid game id = {exitMessage.GameId}");
                }
                else
                {
                    game.Exit();
                    reply = new AckMessage(game.Id);
                    game.Student.IncExitCount();
                    game.LastMessageTime = DateTime.Now;
                }
            }
            else
            {
                Log.InfoFormat("Invalid exitHit message from {0}", exitMessage.SenderEndPoint);
                reply = new ErrorMessage(exitMessage.GameId, @"Invalid exitHint message");
            }
            return reply;
        }

        private void ProcessAckMessage(AckMessage ackMessage)
        {
            if (ackMessage.IsValid)
            {
                var game = Game.FindGame(ackMessage.GameId);
                if (game == null)
                {
                    Log.InfoFormat("Invalid Game Id {0} from {1}", ackMessage.GameId, ackMessage.SenderEndPoint);
                }
                else
                {
                    game.GotAck();
                    game.Student.IncHeartbeatCount();
                    game.LastMessageTime = DateTime.Now;
                }
            }
            else
            {
                Log.InfoFormat("Invalid ack message from {0}",ackMessage.SenderEndPoint);
            }
        }

        private Message ProcessGetStatusMessage(GetStatusMessage getStatusMessage)
        {
            if (!getStatusMessage.IsValid) return null;

            Log.InfoFormat("Process GetStatusMessage");

            var statusMessage = new StatusMessage()
            {
                ActiveGameCount = Game.ActiveGameCount
            };

            var nextId = 1;
            var players = Player.KnownPlayers;
            Log.Debug($"Number of player {players.Count}");

            foreach (var s in players)
            {
                var info = new StatusMessage.PlayerInfo()
                {
                    Id = nextId++,
                    Alias = s.Alias,
                    GameCount = s.GameCount,
                    GuessCount = s.GuessCount,
                    HintCount = s.HintCount,
                    ExitCount = s.ExitCount,
                    HeartbeatCount = s.HeartbeatCount,
                    HighScore = s.HighScore,
                    LastMessage = s.LastMessage
                };
                Log.InfoFormat($"Process add Player to StatusMessage {info}");

                statusMessage.Students.Add(info);
            }
            Message reply = statusMessage;
            return reply;
        }

        private bool IsAws(IPAddress address)
        {
            if (address == null) return false;

            var result = false;
            try
            {
                var host = Dns.GetHostEntry(address.ToString());
                result = host.HostName.Contains("amazonaws.com");
            }
            catch (Exception)
            {
                // ignore
            }
            return result;

        }

        #endregion

    }
}
