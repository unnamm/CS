using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Mediator
    {
        static Mediator Instance = new();

        private List<User> _list = [];

        void Register(User u) => _list.Add(u);
        void UnRegister(User u) => _list.Remove(u);
        void Notify(User sender, string msg)
        {
            foreach (var item in _list)
            {
                if (item == sender)
                    continue;

                //mediator process
                if (item.Name.Length > 5)
                    item.Receive(msg);
                else
                    item.Receive("short");
            }
        }

        class User : IDisposable
        {
            public User() => Mediator.Instance.Register(this);
            public string Name { get; set; } = string.Empty;
            public void Dispose() => Mediator.Instance.UnRegister(this);
            public void Receive(string msg) => Console.WriteLine(msg);
            public void Send(string msg) => Mediator.Instance.Notify(this, msg);
        }

        public static void Sample()
        {
            using User u1 = new() { Name = "1" };
            using User u2 = new() { Name = "longlongname" };
            using User u3 = new() { Name = "3" };

            u1.Send("msg");
            u2.Send("data");
        }
    }
}
