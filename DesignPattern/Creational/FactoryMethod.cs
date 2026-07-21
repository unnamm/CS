using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Creational
{
    public class FactoryMethod
    {
        abstract class Factory
        {
            protected abstract object Create();

            public string CreateString() => Create().ToString()!;
        }

        class StringFactory : Factory
        {
            protected override object Create() => "string";
        }
        class IntegerFactory : Factory
        {
            protected override object Create() => 1;
        }

        public static void Sample()
        {
            Factory f = new StringFactory();
            var str1 = f.CreateString();
            f = new IntegerFactory();
            var str2 = f.CreateString();
        }
    }
}
