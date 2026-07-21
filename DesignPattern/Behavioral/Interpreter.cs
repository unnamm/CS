using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Interpreter
    {
        interface IExpression
        {
            int Interpret();
        }

        class Number : IExpression
        {
            private int _number;
            public Number(int number) => _number = number;
            public int Interpret() => _number;
        }
        class Add : IExpression
        {
            private IExpression _left, _right;
            public Add(IExpression left, IExpression right)
            {
                _left = left;
                _right = right;
            }
            public int Interpret() => _left.Interpret() + _right.Interpret();
        }

        public static void Sample()
        {
            var a = new Number(3);
            var b = new Number(4);
            var sum = new Add(a, b);
            var sum2 = new Add(a, sum);

            Console.WriteLine(sum);
        }
    }
}
