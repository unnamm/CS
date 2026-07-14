using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Observer3
    {
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

        class Class1 : IRecipient<string>
        {
            public Class1() => WeakReferenceMessenger.Instance.Register(this);
            public void Receive(string data) => Console.WriteLine(data);
        }

        class Class2 : IRecipient<int>
        {
            public Class2() => WeakReferenceMessenger.Instance.Register(this);
            public void Receive(int data) { }
        }
        public static void Sample3()
        {
            Class1 c1 = new();
            Class1 c2 = new();
            Class2 c3 = new();
            WeakReferenceMessenger.Instance.Send("test");
            WeakReferenceMessenger.Instance.Send(123);
        }
    }
}
