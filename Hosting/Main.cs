using Hosting.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting
{
    public class Main
    {
        public static async Task Sample()
        {
            var viewProvider = new ViewLoggerProvider(action => action());
            //var viewProvider = new ViewLoggerProvider(action => System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(action)); //ui

            var builder = Host.CreateApplicationBuilder();
            builder.Logging
                //.SetMinimumLevel(LogLevel.Trace) //default is info
                //.AddDebug() //this is default
                //.AddConsole() //this is default
                .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning) //default is info
                .AddProvider(new FileLoggerProvider($"D:/logs/{DateTime.Now:yyyy-MM-dd}.txt")) //write file logger
                .AddProvider(viewProvider)
                .AddSimpleConsole(o => o.IncludeScopes = true); //default is false
            builder.Services
                .AddSingleton(viewProvider) //add ui log provider
                .AddSingleton<SingletonClass>()
                .AddTransient<TransientClass>()
                .AddHostedService<RunClass>();
            using var host = builder.Build();
            await host.RunAsync(); //run IHostedService.StartAsync()
        }
    }
}
