
namespace Lib.CS
{
    internal class TupleProcess
    {
        public void F()
        {
            var v = f();
            var item1 = v.Item1;
            var item2 = v.Item2;

            var v2 = f2();
            var item3 = v2.Item1;
            var item4 = v2.Item2;
        }

        private static Tuple<int, string> f()
        {
            return Tuple.Create(1, "a");
        }

        private static (int, string) f2()
        {
            return (5, "aa");
        }
    }
}
