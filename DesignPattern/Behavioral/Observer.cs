using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Observer
    {
        //pattern1
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

        //pattern2
        public class Publisher2
        {
            public event Action<string>? OnNotify;
            public void Notify(string data) => OnNotify?.Invoke(data);
        }
        class Class2
        {
            public void Update(string data) => Console.WriteLine(data);
        }
        public static void Sample2()
        {
            var p = new Publisher2();
            Class2 c1 = new();
            Class2 c2 = new();
            p.OnNotify += c1.Update;
            p.OnNotify += c2.Update;
            p.Notify("test");
        }

        //pattern3
        interface IRecipient { }
        interface IRecipient<T> : IRecipient
        {
            void Receive(T data);
        }
        class WeakReferenceMessenger
        {
            public static WeakReferenceMessenger Instance = new();

            List<WeakReference<IRecipient>> _list = [];

            public void Register(IRecipient r) => _list.Add(new WeakReference<IRecipient>(r));
            public void UnRegister(IRecipient r) => _list.RemoveAll(weakRef => !weakRef.TryGetTarget(out var target) || target == r);

            public void Send<T>(T data)
            {
                foreach (var weakRef in _list.ToArray())
                {
                    if (weakRef.TryGetTarget(out var target) && target is IRecipient<T> recipient)
                    {
                        recipient.Receive(data);
                    }
                }
            }
        }
        class Class3 : IRecipient<string>
        {
            public Class3() => WeakReferenceMessenger.Instance.Register(this);
            public void Receive(string data) => Console.WriteLine(data);
        }
        class Class4 : IRecipient<int>
        {
            public Class4() => WeakReferenceMessenger.Instance.Register(this);
            public void Receive(int data) { }
        }
        public static void Sample3()
        {
            Class3 c1 = new();
            Class3 c2 = new();
            Class4 c3 = new();
            WeakReferenceMessenger.Instance.Send("test");
            WeakReferenceMessenger.Instance.Send(123);
        }
    }
}
