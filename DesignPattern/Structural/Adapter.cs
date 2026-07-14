using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    public class Adapter
    {
        interface IBehaviour
        {
            void Func();
        }

        class BehaviourAdapter : IBehaviour
        {
            private readonly Legacy _legacy;
            public BehaviourAdapter(Legacy legacy) => _legacy = legacy;
            public void Func() => _legacy.Go();
        }

        class Legacy
        {
            public void Go() { }
        }

        public static void Sample()
        {
            Legacy l = new();
            IBehaviour a = new BehaviourAdapter(l);
            a.Func();
        }
    }
}
