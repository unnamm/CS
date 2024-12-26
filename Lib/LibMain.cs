using Lib.CS;

namespace Lib
{
    public class LibMain
    {
        /// <summary>
        /// main call run()
        /// </summary>
        public async void Run()
        {
            await CustomTaskProcess.Delay(100);
            Console.WriteLine("end");
        }
    }
}
