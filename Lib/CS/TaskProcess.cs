
namespace Lib.CS
{
    internal class TaskProcess
    {
        public void RunAwait() //use await
        {
            asyncFunc();
            asyncFunc2();
            Console.WriteLine("running...");
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

        //low importance
        public async void RunTaskCompletionSource() //TaskCompletionSource: wait return
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

        public async void RunWhen() //task.when
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

        //low importance
        public void RunWait()
        {
            Console.WriteLine("1second");
            Task.Delay(1000).Wait(); // == Thread.Sleep(1000);
            Console.WriteLine("end");
        }

        //low importance
        public void RunResult()
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

        public async void RunCancel()
        {
            CancellationTokenSource cts = new();

            _ = Task.Run(async () =>
            {
                await Task.Delay(1000);
                cts.Cancel();
            }); //async cancel after 1second

            try
            {
                await run7_1(cts.Token); //A task was canceled.
                //await run7_2(cts.Token); //The operation was canceled.

                while (true)
                {
                    await Task.Delay(1);
                    cts.Token.ThrowIfCancellationRequested(); //The operation was canceled.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task run7_1(CancellationToken token)
        {
            await Task.Delay(5000, token); //throw after 1second 
            Console.WriteLine("play after 5second"); //no print
        }
        private async Task run7_2(CancellationToken token)
        {
            while (true)
            {
                await Task.Delay(1);
                token.ThrowIfCancellationRequested();
            }
        }
    }
}
