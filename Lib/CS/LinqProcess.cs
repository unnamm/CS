
namespace Lib.CS
{
    internal class LinqProcess
    {
        public static void Declare()
        {
            int[] arr = [1, 2, 3];
            arr = new int[] { 1, 2, 3 };
            arr = new[] { 1, 2, 3 };

            List<int> list = [1, 2];
            list = new List<int> { 1, 2 };
            list = new() { 1, 2 };
        }

        public static void Select()
        {
            var arr = new int[] { 100, 200, 300 };

            var v1 = arr.Select(x => x + 15); //{115, 215, 315}
            var v2 = arr.Select((x, i) => (x, i)); //{(100,0), (200,1), (300,2)}
        }

        public static void Foreach()
        {
            var arr = new int[] { 1, 2, 3, 4, 5 };
            var list = new List<int>();

            Array.ForEach(arr, list.Add);

            foreach (var x in arr)
            {
                list.Add(x);
            }
        }

        public void Join()
        {
            var arr = new int[] { 100, 200, 300 };

            var str = string.Join(',', arr); //"100,200,300"
        }

        public void Split()
        {
            string str = "a,bc,def";
            var strArray = str.Split(','); //[a, bc, def];
        }

        public void Distinct()
        {
            var arr = new int[] { 1, 2, 3, 1 };
            var arr2 = arr.Distinct(); //[1,2,3]

            var arr3 = new Product[] { new("n1", 1), new("n2", 2), new("n1", 1) };
            var arr4 = arr3.Distinct(); //no distinct
            var arr5 = arr3.Distinct(new ProductComparer());

            var arr6 = arr3.DistinctBy(x => x.Name); //compare Name
            var arr7 = arr3.GroupBy(x => x.Name).Select(x => x.First()); //compare Name
        }

        public class Product
        {
            public string Name { get; set; }
            public int Code { get; set; }

            public Product(string name, int code)
            {
                Name = name;
                Code = code;
            }
        }

        class ProductComparer : IEqualityComparer<Product>
        {
            public bool Equals(Product? x, Product? y)
            {
                if (Object.ReferenceEquals(x, y))
                    return true;

                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                return x.Code == y.Code && x.Name == y.Name;
            }

            public int GetHashCode(Product product)
            {
                if (Object.ReferenceEquals(product, null)) return 0;

                int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();
                int hashProductCode = product.Code.GetHashCode();
                return hashProductName ^ hashProductCode;
            }
        }
    }
}
