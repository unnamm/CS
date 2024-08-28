using System.Net.NetworkInformation;
using System.Numerics;
using System.Threading;

namespace Lib.CS
{
    internal class ThreadSyncProcess
    {
        private object _lockObj = new();
        private int _data = 0;

        private string _lockObj3 = string.Empty;
        private object _lockObj2 = 0;

        public async void Run()
        {
            await Task.WhenAll(Task.Run(repeat), Task.Run(repeat), Task.Run(repeat));
            Console.WriteLine(_data); //30000
        }
        private void repeat()
        {
            for (int i = 0; i < 10000; i++)
            {
                lock (_lockObj)
                {
                    _data++;
                }
            }
        }

        public async void Run2()
        {
            await Task.WhenAll(Task.Run(repeat2), Task.Run(repeat2), Task.Run(repeat2));
            Console.WriteLine(_lockObj2); // no 30000
        }
        private void repeat2()
        {
            for (int i = 0; i < 10000; i++)
            {
                lock (_lockObj2)
                {
                    int temp = (int)_lockObj2;
                    temp++;
                    _lockObj2 = temp;
                }
            }
        }

        public async void Run3()
        {
            await Task.WhenAll(Task.Run(repeat3), Task.Run(repeat3), Task.Run(repeat3));
            Console.WriteLine((_lockObj3).Length); // no 3000
        }
        private void repeat3()
        {
            for (int i = 0; i < 1000; i++)
            {
                lock (_lockObj3)
                {
                    _lockObj3 += "a";
                }
            }
        }
    }


    internal class MutexProcess
    {
        private Mutex _mutex = new(false);
        private int _data = 0;

        public async void Run()
        {
            await Task.WhenAll(Task.Run(repeat), Task.Run(repeat), Task.Run(repeat));
            Console.WriteLine(_data); //60000
        }
        private void repeat()
        {
            for (int i = 0; i < 20000; i++)
            {
                _mutex.WaitOne();
                _data++;
                _mutex.ReleaseMutex(); //use same thread
            }
        }

        public async void RunException()
        {
            _ = Task.Run(repeat2);
            _ = Task.Run(repeat2);
            await Task.Delay(1000);
            _mutex.ReleaseMutex(); //exception
            await Task.Delay(1000);
            _mutex.ReleaseMutex();
        }
        private void repeat2()
        {
            _mutex.WaitOne();
            for (int i = 0; i < 20000; i++)
            {
                _data++;
            }
        }

        public void RunLockStack()
        {
            Console.WriteLine("lock");
            _mutex.WaitOne();
            _mutex.WaitOne();

            Thread.Sleep(100);

            _mutex.ReleaseMutex();
            _mutex.ReleaseMutex();
            Console.WriteLine("release"); //end
        }
    }

    internal class SemaphoreProcess
    {
        private Semaphore _semaphore = new(1, 1);
        private int _data = 0;

        public async void Run()
        {
            await Task.WhenAll(Task.Run(repeat), Task.Run(repeat), Task.Run(repeat));
            Console.WriteLine(_data); //60000
        }
        private void repeat()
        {
            for (int i = 0; i < 20000; i++)
            {
                _semaphore.WaitOne();
                _data++;
                _semaphore.Release();
            }
        }

        public async void Run2()
        {
            _ = Task.Run(repeat2);
            _ = Task.Run(repeat2);
            await Task.Delay(1000);
            _semaphore.Release(); //available
            await Task.Delay(1000);
            _semaphore.Release();
            Console.WriteLine(_data); //40000
        }
        private void repeat2()
        {
            _semaphore.WaitOne();
            for (int i = 0; i < 20000; i++)
            {
                _data++;
            }
        }

        public void RunLockStack()
        {
            Task.Run(() =>
            {
                Console.WriteLine("lock1");
                _semaphore.WaitOne();
                Console.WriteLine("lock2");
                _semaphore.WaitOne(); //wait infinity, other thread release

                Thread.Sleep(10);

                Console.WriteLine("lock1 release");
                _semaphore.Release();

                Console.WriteLine("release");
            });

            Thread.Sleep(1000);
            _semaphore.Release();
        }
    }

    internal class MutexError
    {
        Mutex _mutex = new();

        Semaphore _semaphore = new(1, 1);

        public void Run()
        {
            Test();
            Test(); //no wait, mutex no work, throw
        }

        public void Run2()
        {
            Test2();
            Test2(); //wait, run after release
        }

        private async void Test()
        {
            Console.WriteLine("start wait");
            _mutex.WaitOne();
            Console.WriteLine("wait 1");
            await Task.Delay(1000);
            _mutex.ReleaseMutex();
            Console.WriteLine("release");
        }
        private async void Test2()
        {
            Console.WriteLine("start wait");
            _semaphore.WaitOne();
            Console.WriteLine("wait 1");
            await Task.Delay(1000);
            _semaphore.Release();
            Console.WriteLine("release");
        }
    }
}
