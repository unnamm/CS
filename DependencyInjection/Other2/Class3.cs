using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection.Other2
{
    internal class Class3 : IClass
    {
        public void Run()
        {
            Console.WriteLine($"run {this}");
        }
    }
}
