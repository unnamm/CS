
namespace Lib.CS
{
    internal class ArrayProcess
    {
        public static void Declare()
        {
            int[] arr = [1, 2, 3];

            List<int> list = [1, 2];
        }

        public static void Select()
        {
            var arr = new int[] { 100, 200, 300 };

            IEnumerable<int> v1 = arr.Select(x => x + 15); //{115, 215, 315}
            IEnumerable<int> v2 = arr.Select((x, i) => x + i); //{100, 201, 302}, i: 0,1,2,...
        }

        public static void Foreach()
        {
            var arr = new int[] { 1, 2, 3, 4, 5 };
            var list = new List<int>();

            Array.ForEach(arr, list.Add); //count 5

            foreach (var x in arr)
            {
                list.Add(x); //count 10
            }
        }

        public void Join()
        {
            var arr = new int[] { 100, 200, 300 };

            var v1 = string.Join(',', arr); //"100,200,300"
        }
    }
}
