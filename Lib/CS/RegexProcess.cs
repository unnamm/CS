using System.Text.RegularExpressions;

namespace Lib.CS
{
    internal class RegexProcess
    {
        public void F()
        {
            var str = Console.ReadLine();
            var r = new Regex(@"\s");

            if (r.IsMatch(str))
            {
                Console.WriteLine("no space");
            }
            else
            {
                Console.WriteLine("good");
            }

            r = new Regex(@"[a-z|A-Z|0-9|_]+@[a-z|A-Z|0-9]+\.com");
            if (r.IsMatch(str))
            {
                Console.WriteLine("good");
            }
            else
            {
                Console.WriteLine("no email");
            }
        }
    }
}
