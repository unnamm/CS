using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class TemplateMethod
    {
        abstract class Template
        {
            public void Run()
            {
                Open();
                Play();

                if (IsNeedClose())
                    Close();
            }

            protected abstract void Open();
            protected abstract void Play();
            protected abstract void Close();
            protected virtual bool IsNeedClose() => false;
        }

        class Game : Template
        {
            protected override void Close() => Console.WriteLine("close game");
            protected override void Open() => Console.WriteLine("open game");
            protected override void Play() => Console.WriteLine("play game");
        }
        class Car : Template
        {
            protected override void Close() => Console.WriteLine("close car");
            protected override void Open() => Console.WriteLine("open car");
            protected override void Play() => Console.WriteLine("play car");
            protected override bool IsNeedClose() => true;
        }

        public static void Sample()
        {
            Template t = new Game();
            t.Run();
            t = new Car();
            t.Run();
        }
    }
}
