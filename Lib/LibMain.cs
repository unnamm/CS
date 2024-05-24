using Lib.CS;
using Lib.Hosting;

namespace Lib
{
    //main run call public
    //lib test run
    public class LibMain
    {
        public void Run()
        {
            var c = new DependencyInjectionProcess();
            c.F7();
        }
    }
}
