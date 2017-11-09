using System;
using System.ServiceProcess;
using System.Threading;

using log4net;
using Common;

namespace WordGuessServer
{
    public class WordGuessService : ServiceBase
    {
        #region Private Data Members
        private static readonly ILog Log = LogManager.GetLogger(typeof(Worker));
        private readonly ServerSettings _settings = new ServerSettings();
        private Communicator _comm;
        private Listener _listener;
        private Worker _worker;
        private Cleaner _cleaner;
        private HeartbeatGenerator _heartbeatGenerator;
        #endregion

        public WordGuessService()
        {
            Log.Info("Initializing WordGuessService");
            ServiceName = "WordGuessService";
            AutoLog = false;
            WordDictionary.Load();
        }

        public static void StandAloneStartup(string[] args)
        {
            WordGuessService service = new WordGuessService();
            service.OnStart(args);

            string exitCommand = string.Empty;
            while (string.IsNullOrEmpty(exitCommand) || exitCommand.Trim().ToUpper() != "EXIT")
            {
                Console.WriteLine(@"Enter EXIT to stop the server");
                exitCommand = Console.ReadLine();
                Log.DebugFormat(@"exitCommand={0}", exitCommand);
            }
            Console.WriteLine(@"Exiting...");
            service.OnStop();

        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Starting WordGuessService");
            try
            {
                string studentResultsFile = _settings.StudentResultsFile;
                if (args != null && args.Length > 0)
                    studentResultsFile = args[0];

                _comm = new Communicator(_settings.Port);
                _listener = new Listener(_comm, _settings.Timeout);
                _worker = new Worker(_comm, _settings.Timeout, studentResultsFile);
                _cleaner = new Cleaner(_settings.CleanupTime);
                _heartbeatGenerator = new HeartbeatGenerator(_comm, _settings.Heartbeat, Game.SendOutHeartbeats);
                _listener.Start();
                _worker.Start();
                _cleaner.Start();
                _heartbeatGenerator.Start();
            }
            catch (Exception err)
            {
                Log.Fatal(err.ToString());
            }
        }

        protected override void OnStop()
        {
            Log.Info("Stopping WordGuessService");
            if (_heartbeatGenerator != null && _heartbeatGenerator.IsRunning)
                _heartbeatGenerator.Stop();

            if (_cleaner != null && _cleaner.IsRunning)
                _cleaner.Stop();

            if (_worker != null && _worker.IsRunning)
                _worker.Stop();

            if (_listener != null && _listener.IsRunning)
                _listener.Stop();

            Log.Info("Waiting for all threads to stop");
            WaitForExit();
            Log.Info("All threads have stopped");
        }

        private void WaitForExit()
        {
            while (_listener != null && _listener.IsRunning)
                Thread.Sleep(100);

            while (_worker != null && _worker.IsRunning)
                Thread.Sleep(100);

            while (_cleaner != null && _cleaner.IsRunning)
                Thread.Sleep(100);

            while (_heartbeatGenerator != null && _heartbeatGenerator.IsRunning)
                Thread.Sleep(100);
        }

    }
}
