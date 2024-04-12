using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Principal;

namespace Lib.CS
{
    [SupportedOSPlatform(WINDOWS)] //use windows os
    internal class BuiltInRoleProcess
    {
        public const string WINDOWS = "windows";

        public async Task GetAdministrator()
        {
            if (!isAdministrator())
            {
                const string EXE = "Run.exe";

                var v1 = System.IO.Directory.GetCurrentDirectory();
                var v2 = System.Environment.CurrentDirectory;

                if (v1 != v2)
                {
                    throw new Exception("want comment");
                }

                var procInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = Path.Combine(v2, EXE),
                    WorkingDirectory = Environment.CurrentDirectory,
                    Verb = "runas",
                };

                try
                {
                    await Task.Run(() =>
                    {
                        Process.Start(procInfo); //run new administrator program
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception("manage fail", ex);
                }
                //success
                Environment.Exit(0); //exit before program
            }
        }

        private bool isAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();

            if (identity != null)
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }
    }
}
