using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lib.CS
{
    [Obsolete("your message")]
    [My(1, 2)]
    internal class AttributeProcess
    {
        [My(3, 4)]
        public string Property1 = string.Empty; //is not property

        [My(5, 6)]
        public string Property2 { get; set; } = string.Empty;

        public static void Example()
        {
            var classAttribute = typeof(AttributeProcess).GetCustomAttributes(false);
            foreach (var attribute in classAttribute)
            {
                if (attribute is MyAttribute my)
                {
                    Console.WriteLine($"{nameof(AttributeProcess)} attribute / Data1: {my.Data1} Data2: {my.Data2}");
                }
                if (attribute is ObsoleteAttribute ob)
                {
                    Console.WriteLine($"{nameof(AttributeProcess)} attribute Obsolete message: {ob.Message}");
                }
            }

            var properties = typeof(AttributeProcess).GetProperties();
            foreach (var property in properties)
            {
                var propertyAttributes = property.GetCustomAttributes(false);
                foreach (var attribute in propertyAttributes)
                {
                    if (attribute is MyAttribute my)
                    {
                        Console.WriteLine($"{property.Name} attribute / Data1: {my.Data1} Data2: {my.Data2}");
                    }
                }
            }
        }
    }

    class MyAttribute : Attribute
    {
        public int Data1;
        public int Data2 { get; set; }

        public MyAttribute(int data1, int data2)
        {
            Data1 = data1;
            Data2 = data2;
        }
    }
}
