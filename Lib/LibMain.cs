using Lib.CS;
using Lib.Other;
using System.Diagnostics;

namespace Lib
{
    /// <summary>
    /// main call Run()
    /// </summary>
    public class LibMain
    {
        public void Run()
        {
            var v = Test.Instance;
        }

        class Test
        {
            private static readonly Lazy<Test> _lazy = new Lazy<Test>(System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            public static Test Instance => _lazy.Value;

            private string data = null;

            public Test()
            {
                data = "aaaaa";
            }
        }
    }


}
