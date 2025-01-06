using Lib.CS;
using Lib.Other;

namespace Lib
{
    /// <summary>
    /// main call Run()
    /// </summary>
    public class LibMain
    {
        public void Run()
        {
            StarForce sf = new();

            int sum = 0;
            int count = 10000;

            for (int i = 0; i < count; i++)
            {
                sum += sf.Run(22, true);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine((double)sum / count);
        }
    }
}
