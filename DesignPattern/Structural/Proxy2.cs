using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    public class Proxy2 //delay create
    {
        interface IObj
        {
            void Set();
        }

        class Obj : IObj
        {
            public void Set() { }
        }

        class Obj2 : IObj
        {
            private IObj? _obj;

            Func<IObj> _factory;

            public Obj2(Func<IObj> factory) => _factory = factory;

            public void Set()
            {
                _obj ??= _factory();
                _obj.Set();
            }
        }

        public static void Sample()
        {
            Obj2 o2 = new(Build);
            o2.Set();
        }

        private static Obj Build()
        {
            return new Obj();
        }
    }
}
