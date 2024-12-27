using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lib.Hosting
{
    /// <summary>
    /// Microsoft.Extensions.Hosting
    /// </summary>
    internal class DependencyInjectionProcess
    {
        public static void Run()
        {
            var builder = Host.CreateApplicationBuilder();
            var services = builder.Services;

            services.AddSingleton<CC>(); //singleton is only one
            //services.AddTransient<CC>(); //Transient is new object, when GetService or auto injection

            //interface manage
            bool isValue = true;
            _ = isValue ? services.AddSingleton<II, AA>() : services.AddSingleton<II, BB>();

            using var host = builder.Build(); //build

            var ii = host.Services.GetService<II>(); //true is AA, false is BB
        }

        interface II { }

        class AA : II
        {
            public AA(CC c) //auto injection
            {
                c.Run();
            }
        }

        class BB : II { }

        class CC
        {
            //public CC(AA a) { } //this is circular reference
            public void Run() { }
        }
    }
}
