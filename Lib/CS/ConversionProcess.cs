
namespace Lib.CS
{
    internal class ConversionProcess
    {
        public class Data
        {
            public double Value;

            public Data(int a)
            {
                Value = a;
            }

            public static explicit operator int(Data a) => (int)a.Value;
            public static implicit operator Data(int a) => new(a);
        }

        public class Data2
        {
            public double Value;

            public Data2(int a)
            {
                Value = a;
            }

            public static implicit operator int(Data2 a) => (int)a.Value;
            public static explicit operator Data2(int a) => new(a);
        }

        public void F()
        {
            int a = 5;
            Data d = a;
        }

        public void F2()
        {
            Data d = new(5);
            int i = (int)d; //int i = d; error
        }

        public void F3()
        {
            int a = 5;
            Data2 d = (Data2)a; //Data2 d = a; error
        }

        public void F4()
        {
            Data2 d = new(5);
            int i = d;
        }
    }
}
