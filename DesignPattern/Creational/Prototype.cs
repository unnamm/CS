using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Creational
{
    public class Prototype
    {
        interface IPrototype<T>
        {
            T Clone();
        }

        class Class1 : IPrototype<Class1>
        {
            private int data;
            private int[] data2;

            public Class1(int data, int[] data2)
            {
                this.data = data;
                this.data2 = data2;
            }

            public Class1 Clone()
            {
                int[] copy = new int[data2.Length];
                Array.Copy(data2, copy, data2.Length);
                return new Class1(data, copy);
            }
        }

        public static void Sample()
        {
            var c1 = new Class1(1, [1, 2, 3]);
            Class1 c2 = c1.Clone();
        }
    }
}
