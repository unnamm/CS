using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjection.Other;
using DependencyInjection.Other2;

namespace DependencyInjection
{
    internal class RunClass
    {
        private readonly Class1 _c1;
        private readonly Class2 _c2;
        private readonly IClass _ic;

        public RunClass(Class1 c1, Class2 c2, IClass ic)
        {
            _c1 = c1;
            _c2 = c2;
            _ic = ic;
        }

        public void Run()
        {
            _c1.Run();
            _c2.Run();
            _ic.Run();
        }
    }
}
