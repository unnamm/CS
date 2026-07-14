using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Observer1
    {
        public class Publisher
        {
            private List<IObserver> _list = [];
            public void Register(IObserver observer) => _list.Add(observer);
            public void UnRegister(IObserver observer) => _list.Remove(observer);

            public void Notify(string data)
            {
                foreach (var item in _list)
                {
                    item.Update(data);
                }
            }
        }

        public interface IObserver
        {
            void Update(string data);
        }

        class Class1 : IObserver
        {
            public Class1(Publisher publisher) => publisher.Register(this);
            public void Update(string data) => Console.WriteLine(data);
        }

        public static void Sample()
        {
            Publisher p = new();
            Class1 c1 = new(p);
            Class1 c2 = new(p);
            p.Notify("test");
        }
    }
}
