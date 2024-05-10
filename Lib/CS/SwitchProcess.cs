using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    public class SwitchProcess
    {
        public static int GetValue(int value) //return value
        {
            int localFunc() => 332;

            int result = value switch
            {
                1 => 5,
                2 => 47,
                3 => localFunc(),
                _ => throw new NotImplementedException()
            };
            return result;
        }

        public void Run(int value) //run void function
        {
            (value switch
            {
                1 => (Action)run1, //need (Action)
                2 => run2,
                3 => run3,
                _ => throw new NotImplementedException()
            })();
        }

        private void run1() { }
        private void run2() { }
        private void run3() { }
    }
}
