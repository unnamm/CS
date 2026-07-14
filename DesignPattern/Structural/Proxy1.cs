using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    public class Proxy1 //access limit
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

            public Obj2(IObj? obj) => _obj = obj;

            public void Set()
            {
                if (_obj == null)
                    return;

                _obj.Set();
            }
        }

        public static void Sample()
        {
            Obj o = new();

            Obj2 o2 = new(o);
            o2.Set(); //run o.Set()

            Obj2 o3 = new(null);
            o3.Set(); //return
        }
    }
}
