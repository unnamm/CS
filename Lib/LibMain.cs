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
            ushort b = 0xFF0F;
            var arr = DataConvertProcess.GetBitArrayFromUshort(b);
            var value = DataConvertProcess.GetushortFromBitArray(arr);
        }
    }
}
