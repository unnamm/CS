using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Strategy
    {
        interface ICalc
        {
            decimal Calc(decimal value);
        }

        class Up : ICalc
        {
            public decimal Calc(decimal value) => value * 10;
        }

        class Down : ICalc
        {
            public decimal Calc(decimal value) => value * 0.1m;
        }

        class Calc
        {
            private decimal _value;
            private ICalc? _calc;

            public Calc(decimal value) => _value = value;

            public void Set(ICalc calc)
            {
                _calc = calc;
            }

            public decimal Run()
            {
                if (_calc == null)
                    return _value;

                _value = _calc.Calc(_value);
                return _value;
            }
        }

        public static void Sample()
        {
            Calc c = new(100m);
            c.Set(new Up());
            c.Run();
            c.Set(new Down());
            c.Run();
        }
    }
}