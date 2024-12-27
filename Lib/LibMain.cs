using Lib.CS;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Lib
{
    public class LibMain
    {
        /// <summary>
        /// main call run()
        /// </summary>
        public void Run()
        {
            LogProcess log = new();
            log.Initialize(@"D:\Folder");
            Stopwatch stopwatch = Stopwatch.StartNew();
            log.Write("aaaaaaa");
            Console.WriteLine(stopwatch);
        }

    }
}
