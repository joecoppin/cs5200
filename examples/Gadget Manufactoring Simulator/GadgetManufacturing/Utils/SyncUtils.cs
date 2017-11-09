using System;
using System.Threading;

namespace Utils
{
    public static class SyncUtils
    {
        /// <summary>
        /// This functions implements a busy wait or busy loop for a condition represented by a bool function.  Busy loop
        /// will only wait for a certain amount of time (e.g., the timeout) before giving up.  If the timeout is less than
        /// or equal to zero, then it will not wait at all on return the status of the condition.  If the timeout has not
        /// been executeded and the condition is true, then it will cause the current thread to sleep for a specified number
        /// of milliseconds (e.g., intermediateSleepTime) and in doing do, will give up the processor.
        /// 
        /// NOTE: The function currently has an error.
        /// </summary>
        /// <param name="condition">a bool function that tests the desired condition</param>
        /// <param name="timeout">an approximate maxmim amount of time to wait in ms.  Note that this is not a percise
        /// limit and other timing should not be based on true amount of elapsed time.</param>
        /// <param name="intermediateSleepTime">an intermediate sleep time in ms.  If the condidtion is not true,
        /// then the thread will relinquesh the CPU for this amount of time before checking the condition again.  If the
        /// intermediate sleep time is 0, then the current thread will give up the CPU but will no be forced to sleep.</param>
        /// <returns>True if the condition was meet, otherwise false</returns>
        public static bool WaitForCondition(Func<bool> condition, int timeout = 1000, ushort intermediateSleepTime = 10)
        {
            bool result = false;

            int remainingTime = timeout;
            while (remainingTime > 0 && !(result=condition()))
            {
                Thread.Sleep(intermediateSleepTime);
                remainingTime -= intermediateSleepTime;
            }

            return result;
        }
    }
}