using System.Diagnostics;

namespace Lib.CS
{
    internal class ProcessProcess
    {
        public void Run()
        {
            const string FOLDER = @"D:\Folder";
            const string FILE = "ExeFile.exe";

            //run(FOLDER, FILE);
            runAsync(FOLDER, FILE, 5000);
        }

        private void run(string folder, string file)
        {
            ProcessStartInfo psi = new()
            {
                FileName = Path.Combine(folder, file),
                WorkingDirectory = folder, //new process set path
            };
            var pro = Process.Start(psi);
            pro.WaitForInputIdle(); //wait process open
        }

        private async void runAsync(string folder, string file, int timeout)
        {
            var task = Task.Run(() =>
            {
                run(folder, file);
            });

            var end = await Task.WhenAny(task, Task.Delay(timeout));

            if (end != task)
            {
                throw new TimeoutException();
            }

            //run after open
        }
    }
}
