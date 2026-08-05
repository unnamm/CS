using Hosting.Base;
using Hosting.Config;
using Microsoft.Extensions.Configuration;
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

            builder.Configuration
                .AddJsonFile("Config/configValue.json", optional: false, reloadOnChange: true)
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);

            builder.Logging
                //.SetMinimumLevel(LogLevel.Trace) //default is info
                //.AddDebug() //this is default
                //.AddConsole() //this is default
                //.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning) //default is info
                .AddSimpleConsole(o => o.IncludeScopes = true); //show scope to console

            builder.Services
                .AddSingleton<ViewLoggerProvider>()
                .AddSingleton<FileLoggerProvider>()
                .AddSingleton<ILoggerProvider>(sp => sp.GetRequiredService<ViewLoggerProvider>())
                .AddSingleton<ILoggerProvider>(sp => sp.GetRequiredService<FileLoggerProvider>())
                .Configure<Appsettings>(builder.Configuration.GetSection("Appsettings"))
                .Configure<ConfigValue>(builder.Configuration.GetSection("ConfigValue"))
                .AddTransient<TransientClass>()
                .AddKeyedSingleton<SingletonClass>("key1")
                .AddKeyedSingleton<SingletonClass>("key2")
                .AddHostedService<RunClass>();

            using var host = builder.Build();
            //host.Services.GetRequiredService<ViewLoggerProvider>().SetInvoker(action => System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(action)); //ui
            await host.RunAsync(); //run IHostedService.StartAsync()

            Console.WriteLine("push Ctrl+C");
        }
    }
}
