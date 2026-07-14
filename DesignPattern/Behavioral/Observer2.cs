using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Observer2
    {
        public class Publisher
        {
            public event Action<string>? OnNotify;
            public void Notify(string data) => OnNotify?.Invoke(data);
        }

        class Class1
        {
            public void Update(string data) => Console.WriteLine(data);
        }

        public static void Sample2()
        {
            var p = new Publisher();
            Class1 c1 = new();
            Class1 c2 = new();
            p.OnNotify += c1.Update;
            p.OnNotify += c2.Update;
            p.Notify("test");
        }
    }
}
