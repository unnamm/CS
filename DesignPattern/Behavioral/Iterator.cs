using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Iterator
    {
        public static void Sample()
        {
            IEnumerable<int> ii;
            ii = new List<int>() { 1, 2 };
            Run(ii);
            ii = [1, 2, 3];
            Run(ii);
            ii = new int[] { 1, 2, 3, 4 };
            Run(ii);
        }

        private static void Run(IEnumerable<int> ii)
        {
            foreach (var i in ii)
            {
                Console.WriteLine(i);
            }
        }
    }
}
