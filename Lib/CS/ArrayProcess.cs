
namespace Lib.CS
{
    internal class ArrayProcess
    {
        public static void Select()
        {
            var arr = new int[] { 100, 200, 300 };

            IEnumerable<int> v1 = arr.Select(x => x + 15); //{115, 215, 315}
            IEnumerable<int> v2 = arr.Select((x, i) => x + i); //{100, 201, 302}, i: 0,1,2,...
        }

        public static void Select2()
        {
            var arr = new int[] { 1, 2, 3, 4, 5 };
            var list = new List<int>();

            _ = arr.Select(x => { list.Add(x); return x; }); //no run ramda inner
            var count1 = list.Count(); //count is 0

            _ = arr.Select(x => { list.Add(x); return x; }).ToArray(); //run inner
            var count2 = list.Count(); //count is arr length
        }

        public void Join()
        {
            var arr = new int[] { 100, 200, 300 };

            string v1 = string.Join(',', arr); //100,200,300
        }
    }
}
