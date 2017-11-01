using System.Threading;

using Common;

namespace WordGuessServer
{
    public class Cleaner : BackgroundThread
    {
        #region Private data members
        private readonly int _cleanIntervalTime;
        #endregion

        #region Constructors and destruction
        public Cleaner(int cleanIntervalTime)
        {
            _cleanIntervalTime = cleanIntervalTime;
        }
        #endregion

        #region Public Methods

        public override string ThreadName()
        {
            return "Cleaner";
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
            while (KeepGoing)
            {
                int remainingTime = _cleanIntervalTime;
                while (remainingTime > 0 &&  KeepGoing)
                {
                    Thread.Sleep(100);
                    remainingTime -= 100;
                }
                if (KeepGoing)
                    Game.CleanupGames();
            }
        }

        #endregion

    }
}
