using System.Diagnostics;

namespace Lib.CS
{
    internal class ProcessProcess
    {
        public static async void RunAsync(string filePath, int timeout)
        {
            if (filePath.Contains(".exe") == false)
            {
                throw new Exception("run exe file");
            }

            ProcessStartInfo psi = new()
            {
                FileName = filePath,
                WorkingDirectory = Path.GetDirectoryName(filePath), //new process set path
            };

            var pro = Process.Start(psi) ?? throw new ArgumentNullException("process null");

            var task = Task.Run(() =>
            {
                pro.WaitForInputIdle(); //wait process open
            });

            var end = await Task.WhenAny(task, Task.Delay(timeout));

            if (end != task)
            {
                throw new TimeoutException();
            }

            Console.WriteLine("open complete");

            await pro.WaitForExitAsync(); //wait when exit process

            Console.WriteLine("exit event");
        }
    }
}
