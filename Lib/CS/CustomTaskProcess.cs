using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    internal class CustomTaskProcess
    {
        private MyAwaiter _awaiter;

        private CustomTaskProcess()
        {
            _awaiter = new();
        }

        public MyAwaiter GetAwaiter() => _awaiter;

        public void Finish()
        {
            if (_awaiter.CompleteAction == null)
            {
                Task.Run(() =>
                {
                    Console.WriteLine("timer is faster then await MyTask.Delay");
                    SpinWait.SpinUntil(() => _awaiter.CompleteAction != null); //wait not null
                    _awaiter.CompleteAction!();
                });
                return;
            }

            _awaiter.CompleteAction();
        }

        public static CustomTaskProcess Delay(int milli)
        {
            var awaitable = new CustomTaskProcess();
            new Timer(x => awaitable.Finish(), null, milli, Timeout.Infinite);
            return awaitable;
        }
    }

    /// <summary>
    /// custom awaiter
    /// </summary>
    internal class MyAwaiter : System.Runtime.CompilerServices.INotifyCompletion
    {
        public Action? CompleteAction;

        /// <summary>
        /// need, init check
        /// </summary>
        public bool IsCompleted => false;

        /// <summary>
        /// return value after await
        /// </summary>
        //public int GetResult() => 1;
        public void GetResult() { }

        /// <summary>
        /// Schedules the continuation action that's invoked when the instance completes.
        /// </summary>
        /// <param name="continuation">The action to invoke when the operation completes.</param>
        public void OnCompleted(Action continuation)
        {
            CompleteAction = continuation; //call getresult, break await
        }
    }
}
