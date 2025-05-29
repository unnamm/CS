
namespace Lib.CS
{
    internal class TupleProcess
    {
        public void F()
        {
            var v = f();
            var item11 = v.Item1;
            var item12 = v.Item2;

            var v2 = f2();
            var item21 = v2.Item1;
            var item22 = v2.Item2;

            var v3 = f3();
            var item31 = v3.A;
            var item32 = v3.B;

            var (A, B) = f3();
            var item33 = A;
            var item34 = B;
        }

        private static Tuple<int, string> f()
        {
            return Tuple.Create(1, "one");
        }

        private static (int, string) f2()
        {
            return (2, "two");
        }

        private static (int A, string B) f3()
        {
            return (3, "three");
        }
    }
}
