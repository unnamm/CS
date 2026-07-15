using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class State
    {
        interface ILight
        {
            ILight On();
        }

        class Green : ILight
        {
            public ILight On()
            {
                Console.WriteLine("green");
                return new Yellow();
            }
        }

        class Yellow : ILight
        {
            public ILight On()
            {
                Console.WriteLine("yellow");
                return new Red();
            }
        }

        class Red : ILight
        {
            public ILight On()
            {
                Console.WriteLine("red");
                return new Green();
            }
        }

        class Blinker
        {
            private ILight _current;

            public Blinker(ILight currnet) => _current = currnet;

            public ILight Blink()
            {
                _current = _current.On();
                return _current;
            }
            public ILight CurrentColor() => _current;
        }

        public static void Sample()
        {
            Blinker blinker = new(new Green());
            blinker.Blink();
            blinker.Blink();
            blinker.Blink();
            var currentColor = blinker.CurrentColor();
        }
    }
}
