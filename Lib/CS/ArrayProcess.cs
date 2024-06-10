
namespace Lib.CS
{
    internal class ArrayProcess
    {
        public void Select()
        {
            var arr = new int[] { 100, 200, 300 };

            IEnumerable<int> v1 = arr.Select(x => x + 15); //{115, 215, 315}
            IEnumerable<int> v2 = arr.Select((x, i) => x + i); //{100, 201, 302}, i: 0,1,2,...
        }

        private class Temp
        {
            public int F()
            {
                int data = 1;
                Console.WriteLine(data);
                return data;
            }
        }

        public void Select2()
        {
            Temp[] values = new[] { new Temp(), new Temp() };
            var v = values.Select(x => x.F()); //{1, 1}; //no run Console.WriteLine
            values.Select(x => x.F()).ToArray(); // {1, 1}; // run Console.WriteLine
        }

        public void Join()
        {
            var arr = new int[] { 100, 200, 300 };

            string v1 = string.Join(',', arr); //100,200,300
        }
    }
}
