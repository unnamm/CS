namespace Lib.CS
{
    internal class StringFormatProcess
    {
        public void Interpolation()
        {
            int two = 2;
            int three = 0xAB;

            var str = $"two: {two:00}"; //2 number
            str += $"three: {three:X04}"; //16 4 number

            str = "two: " + two.ToString("00");
            str += "three: " + three.ToString("X04");
        }
    }
}
