using System.Diagnostics;

namespace Lib.CS
{
    internal class TaskProcess
    {
        public void Example1() //run same time
        {
            Task.Run(F1);
            Task.Run(F2);
        }
        public void Example2()
        {
            _ = F1(); //execute until await is encountered
            F2(); //run next after f1 await
        }
        public void Example3()
        {
            F2(); //no have await
            _ = F1(); //run next after f2 end
        }
        public async Task Example4()
        {
            var task2 = F2();
            var task1 = F1();

            await Task.WhenAll(task1, task2); //wait all
            Console.WriteLine("all end");

            task2 = F2();
            task1 = F1(); //f1 end after any end

            var anyTask = await Task.WhenAny(task1, task2); //wait any
            Console.WriteLine($"any end: {(anyTask == task1 ? "task1" : "task2")}");
        }

        private async Task F1() //check every 500ms
        {
            var watch = Stopwatch.StartNew();
            while (true)
            {
                if (watch.ElapsedMilliseconds >= 2000)
                {
                    Console.WriteLine("end f1");
                    return;
                }
                await Task.Delay(1000);
            }
        }

        private Task F2() //check always
        {
            var watch = Stopwatch.StartNew();
            while (true)
            {
                if (watch.ElapsedMilliseconds >= 2000)
                {
                    Console.WriteLine("end f2");
                    return Task.CompletedTask;
                }
            }
        }

        public Task<int> GetData1() => Task.FromResult(1);

        public async Task<int> GetData2()
        {
            await Task.Delay(1);
            return 2;
        }

        public async Task ExampleCancel()
        {
            CancellationTokenSource cts = new();

            _ = Task.Run(() =>
            {
                while (true)
                {
                    //first
                    if (cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("end");
                        break;
                    }

                    //second
                    try
                    {
                        cts.Token.ThrowIfCancellationRequested(); //make throw
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("end");
                        break;
                    }
                }
            });

            await Task.Delay(1000);
            cts.Cancel(); //cancel after 1000
        }

    }
}
