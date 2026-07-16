using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class ChainResponsibility
    {
        public abstract class Inspect(Inspect? _next)
        {
            public void Process(int value)
            {
                var result = IsProcess(value);
                if (result)
                {
                    Console.WriteLine("success");
                    return;
                }
                if (_next == null)
                {
                    Console.WriteLine("fail");
                    return;
                }
                _next.Process(value);
            }

            protected abstract bool IsProcess(int value);
        }

        class Inspect1(Inspect? next) : Inspect(next)
        {
            protected override bool IsProcess(int value) => value == 1;
        }
        class Inspect2(Inspect? next) : Inspect(next)
        {
            protected override bool IsProcess(int value) => value == 2;
        }

        public static void Sample()
        {
            int data = 2;

            var i = new Inspect1(new Inspect2(null));
            i.Process(data);
            var i2 = new Inspect2(new Inspect1(null));
            i2.Process(data);
        }
    }
}
