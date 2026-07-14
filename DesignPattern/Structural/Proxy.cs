using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    internal class Proxy
    {
        interface IObj
        {
            void Set();
        }

        class Obj : IObj
        {
            public void Set() { }
        }

        class Obj2 : IObj //access limit
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

        class Obj3 : IObj //delay create
        {
            private IObj? _obj;

            Func<IObj> _factory;

            public Obj3(Func<IObj> factory) => _factory = factory;

            public void Set()
            {
                _obj ??= _factory();
                _obj.Set();
            }
        }
    }
}
