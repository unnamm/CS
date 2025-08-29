using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Create
{
    public class Prototype
    {
        interface IPrototype<T>
        {
            T Clone();
        }

        class Class1 : IPrototype<Class1>
        {
            private int data = 1;
            private string data2 = "dd";

            public Class1(int data, string data2)
            {
                this.data = data;
                this.data2 = data2;
            }

            public Class1 Clone()
            {
                return new Class1(data, new string(data2));
            }
        }

        public void Sample()
        {
            var c1 = new Class1(1, "111");
            Class1 c2 = c1.Clone();
        }

    }
}
