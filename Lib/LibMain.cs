using Lib.CS;

namespace Lib
{
    /// <summary>
    /// main call Run()
    /// </summary>
    public class LibMain
    {
        public void Run()
        {
            var time = BuildTimeProcess.GetBuildTime();
            Console.WriteLine(time);
        }
    }
}
