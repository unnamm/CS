using System.Diagnostics;

namespace Lib.CS
{
    internal class ProcessProcess
    {
        public void Run()
        {
            const string FOLDER = @"D:\Folder";
            const string FILE = "ExeFile.exe";

            run(FOLDER, FILE);
        }

        public void run(string folder, string file)
        {
            ProcessStartInfo psi = new()
            {
                FileName = Path.Combine(folder, file),
                WorkingDirectory = folder, //new process set path
            };
            Process.Start(psi);
        }
    }
}
