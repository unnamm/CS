using Hosting.FileLog;
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
            var builder = Host.CreateApplicationBuilder();
            builder.Logging
                //.SetMinimumLevel(LogLevel.Trace) //default is info
                //.AddDebug() //this is default
                //.AddConsole() //this is default
                .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning) //default is info
                .AddSimpleConsole(o => o.IncludeScopes = true) //default is false
                .AddProvider(new FileLoggerProvider($"D:/logs/{DateTime.Now:yyyy-MM-dd}.txt")); //write file logger
            builder.Services
                .AddHostedService<RunClass>()
                .AddSingleton<SingletonClass>()
                .AddTransient<TransientClass>();
            using var host = builder.Build();
            await host.RunAsync(); //run IHostedService.StartAsync()
        }
    }
}
