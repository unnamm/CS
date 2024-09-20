
namespace Lib.CS
{
    internal class TaskProcess
    {
        public static void Example()
        {
            f();

            Task.Run(async () =>
            {
                await getTask();
                Console.WriteLine("delay 1000");
                var value = await getValue();
                Console.WriteLine("get:" + value);
            });

            Console.WriteLine("example end");
        }

        private static async void f()
        {
            await Task.Delay(1000);
            await getTask();
            Console.WriteLine("delay 1000+1000");
        }

        private static Task getTask() => Task.Delay(1000);

        private async static Task<int> getValue()
        {
            await Task.Delay(1000);
            return 1;
        }

        public async static void ExampleCancel()
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
                        cts.Token.ThrowIfCancellationRequested();
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
