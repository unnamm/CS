using Lib.CS;
using Lib.HardwareMonitor;
using Lib.OpenCV;
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
            OpenCVProcess op = new();

            var mat = op.GetMat(@"user\path.bmp");

            Console.WriteLine(op.SearchRectCount(mat));

            op.SearchContours(mat);
            op.MarkRect(ref mat, op.SearchContours(mat), OpenCvSharp.Scalar.Green);
            op.ShowWindow(mat);
        }
    }
}
