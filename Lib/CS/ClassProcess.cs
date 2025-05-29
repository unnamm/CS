using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    internal class ClassProcess
    {
        public required int RequiredData; //new() { RequiredData = 1 }
        public readonly int ReadonlyData = 2;

        public ClassProcess() { }

        public ClassProcess(int readonlyData)
        {
            ReadonlyData = readonlyData;
        }

        public static void Example()
        {
            ClassProcess cp = new() { RequiredData = 1 }; //readonlydata = 2
            ClassProcess cp2 = new(1) { RequiredData = 10 }; //readonlydata = 1
        }
    }
}
