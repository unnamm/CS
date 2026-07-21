using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Creational
{
    public class Singleton
    {
        class Singleton1 //create anytime before access Singleton1
        {
            private static readonly Singleton1 _instance = new();
            public static Singleton1 Instance => _instance;
            private Singleton1() { }
        }
        class Singleton5 //create access Singleton5
        {
            private static readonly Singleton5 _instance = new();
            public static Singleton5 Instance => _instance;
            static Singleton5() { }
        }
        class Singleton3 //delay create
        {
            private static readonly Lazy<Singleton3> _lazy = new(() => new Singleton3());
            public static Singleton3 Instance => _lazy.Value;
            private Singleton3() { }
        }
        class Singleton4 //delay create
        {
            private static Singleton4? _instance;
            private static readonly object _lock = new();
            public static Singleton4 Instance
            {
                get
                {
                    if (_instance == null) //check 1
                    {
                        lock (_lock)
                        {
                            _instance ??= new(); //check 2
                            return _instance;
                        }
                    }
                    else
                    {
                        return _instance;
                    }
                }
            }
            private Singleton4() { }
        }
        class Singleton2 //maybe wrong
        {
            public readonly static Singleton2 Instance = new();

            private Singleton2() { }
        }

        public static void Sample()
        {
            var v = Singleton1.Instance;
            var v2 = Singleton2.Instance;
        }
    }
}
