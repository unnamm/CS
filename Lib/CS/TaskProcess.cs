
namespace Lib.CS
{
    internal class TaskProcess
    {
        public void Run1() //use await
        {
            asyncFunc();
            asyncFunc2();
        }
        private async void asyncFunc()
        {
            var v = await getData(); //wait, run asyncFunc2(), play nextline after getData return
            Console.WriteLine(v); //after 1000
        }
        private async void asyncFunc2()
        {
            var v = await getData(); //wait, run Run1(), play getData() after getData return
            var v2 = await getData(); //wait, play nextline after getData return
            Console.WriteLine(v + v2); //after 2000
        }
        private async Task<int> getData()
        {
            await Task.Delay(1000); //1000 time wait
            return 1;
        }

        private Action _action;

        public async void Run2() //TaskCompletionSource: wait return
        {
            int value = await f(); //wait after setResult TaskCompletionSource
            //Console.WriteLine(value); print 1
        }
        private Task<int> f()
        {
            var tcs = new TaskCompletionSource<int>();

            _action += () => { tcs.SetResult(1); };
            f2(); //delay call callback func
            return tcs.Task;
        }
        private async void f2()
        {
            await Task.Delay(1000);
            _action();
        }

        public async void Run3() //task.when
        {
            var t1 = Task.Delay(1000);
            var t2 = f3();

            t1.Start();
            t2.Start();

            await Task.WhenAny(t1, t2);
            //t1, t2 any complete, do

            await Task.WhenAll(t1, t2);
            //t1, t2 all complete, do
        }
        private async Task<int> f3()
        {
            await Task.Delay(2000);
            return 1;
        }

        CancellationTokenSource _cts = new CancellationTokenSource();
        public async void Run4() //use token
        {
            var token = _cts.Token;
            cancel();
            var res = await Task.Run(run, token);

            if (res == null)
            {
                Console.WriteLine("cancel");
            }
            else
            {
                Console.WriteLine("sum: " + res);
            }
        }
        private async Task<int?> run()
        {
            int sum = 0;
            for (int i = 0; i < 15; i++)
            {
                if (_cts.Token.IsCancellationRequested) //if cancel
                {
                    return null;
                }
                sum += i;
                await Task.Delay(100);
            }
            return sum;
        }
        private async void cancel()
        {
            await Task.Delay(1000);
            _cts.Cancel();
        }

        public void Run5()
        {
            Task.Delay(1000).Wait(); // == Thread.Sleep(1000);
        }

        public void Run6()
        {
            var task = r6();
            //task.Wait(); //even if no use
            var result = task.Result; //wait 1000
        }
        private async Task<int> r6()
        {
            await Task.Delay(1000);
            return 3;
        }
    }
}
