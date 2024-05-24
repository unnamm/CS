using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Lib.Hosting
{
    /// <summary>
    /// Microsoft.Extensions.Hosting (8.0.0)
    /// </summary>
    internal class DependencyInjectionProcess
    {
        public void F1(string[] args) //try
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.TryAddSingleton<IAlpha, A>();
            builder.Services.TryAddSingleton<IAlpha, B>(); //same interface no add

            using IHost host = builder.Build();

            IAlpha al = host.Services.GetService<IAlpha>(); //A
            al.Run();
        }
        public void F2() //add
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddSingleton<IAlpha, A>();
            builder.Services.AddSingleton<IAlpha, B>(); //last add IAlpha

            using IHost host = builder.Build();

            IAlpha al = host.Services.GetService<IAlpha>(); //B
            al.Run();
        }
        public void F3() //singleton
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddSingleton<A>(); //same instance

            using IHost host = builder.Build();

            A a = host.Services.GetService<A>(); //make new instance
            a.data = 5;

            A b = host.Services.GetService<A>(); //get a instance
            Console.WriteLine(b.data); //5
            var c = a == b; //true
        }

        public void F4() //transient
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddTransient<A>(); //make new instance

            using IHost host = builder.Build();

            A a = host.Services.GetService<A>(); //make new instance
            a.data = 5;

            A b = host.Services.GetService<A>(); //make new instance
            Console.WriteLine(b.data); //0
            var c = a == b; //false
        }

        public void F5() //injection
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddSingleton<C>();
            builder.Services.AddSingleton<D>();

            using IHost host = builder.Build();

            D d = host.Services.GetService<D>();
            C c = d.data; //construct auto injection
        }

        public void F6() //no data injection
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            //builder.Services.AddSingleton<C>();
            builder.Services.AddSingleton<D>();

            using IHost host = builder.Build();

            D d = host.Services.GetService<D>(); //System.InvalidOperationException
        }

        public void F7() //circula dependency
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddSingleton<F>();
            builder.Services.AddSingleton<E>();

            using IHost host = builder.Build();
            F f = host.Services.GetService<F>(); //System.InvalidOperationException
        }

        interface IAlpha
        {
            void Run();
        }

        class A : IAlpha
        {
            public int data;
            public void Run() => throw new NotImplementedException();
        }
        class B : IAlpha
        {
            public void Run() => throw new NotImplementedException();
        }
        class C
        {
            //internal C() //host.Services.GetService<C>(); //System.InvalidOperationException
            //{
            //}
            //private C() //host.Services.GetService<C>(); //System.InvalidOperationException
            //{
            //}
            public C() //only public constructor or empty
            {
            }
        }
        class D
        {
            public C data;

            public D(C c) //auto injection
            {
                data = c;
            }
        }

        class E
        {
            public F _f;
            public E(F f)
            {
                _f = f;
            }
        }
        class F
        {
            public E _e;
            public F(E e)
            {
                _e = e;
            }
        }
    }
}
