using System.Diagnostics;

namespace CommandPrompt
{
    public class RunCommand
    {
        public async Task<string> RunAsync(string command)
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            cmd.FileName = "cmd";
            cmd.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.CreateNoWindow = true;
            cmd.UseShellExecute = false;
            cmd.RedirectStandardOutput = true;
            cmd.RedirectStandardInput = true;
            cmd.RedirectStandardError = true;

            using Process process = new Process();
            process.EnableRaisingEvents = false;
            process.StartInfo = cmd;
            process.Start();
            process.StandardInput.Write(command + Environment.NewLine);
            process.StandardInput.Close();

            string result = process.StandardOutput.ReadToEnd();

            await process.WaitForExitAsync();

            return result;
        }
    }
}
