using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    internal abstract class SingletonProcess<T> where T : class, new()
    {
        //private static readonly Lazy<T> _lazy = new();
        private static readonly Lazy<T> _lazy = new Lazy<T>(() => new(), LazyThreadSafetyMode.ExecutionAndPublication);
        public static T Instance => _lazy.Value;
        

        //maybe same ?
        //private static readonly T _instance = new();
        //public static T Instance => _instance;
    }

    internal class MyClass : SingletonProcess<MyClass>
    {
        public int Data;

        public MyClass()
        {
            Data = 1;
        }

        public static void Run()
        {
            var data = MyClass.Instance.Data;
        }
    }
}
