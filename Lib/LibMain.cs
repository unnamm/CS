using Lib.Config;
using Lib.CS;
using Lib.Hosting;
using System.Reflection;

namespace Lib
{
    public class LibMain
    {
        public void Run() //test run
        {
            ProcessProcess.RunAsync(@"F:\hercules_3-2-8.exe", 1000);
        }
    }
}