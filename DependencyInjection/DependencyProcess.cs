using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjection.Other;
using DependencyInjection.Other2;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    public class DependencyProcess
    {
        private IServiceProvider _serviceProvider;

        public DependencyProcess()
        {
            var serviceList = new ServiceCollection();
            serviceList.AddSingleton<RunClass>();
            serviceList.AddSingleton<Class1>();
            serviceList.AddSingleton<Class2>();
            serviceList.AddSingleton<IClass, Class3>();
            //serviceList.AddSingleton<IClass, Class4>(); //IClass is only one
            _serviceProvider = serviceList.BuildServiceProvider();
        }

        public void Play()
        {
            var run = _serviceProvider.GetService<RunClass>()!;
            run.Run();
        }
    }
}
