using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    /// <summary>
    /// main call Run()
    /// </summary>
    public class LibMain
    {
        public void Run()
        {
            DependencyInjection.DependencyProcess process = new();
            process.Play();
        }
    }
}
