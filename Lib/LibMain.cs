using Lib.CS;
using Lib.Other;

namespace Lib
{
    /// <summary>
    /// main call Run()
    /// </summary>
    public class LibMain
    {
        public void Run()
        {
            StarForce sf = new();
            sf.Run(24);
        }
    }
}
