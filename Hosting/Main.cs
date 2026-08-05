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
            var viewProvider = new ViewLoggerProvider(action => action());
            //var viewProvider = new ViewLoggerProvider(action => System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(action)); //ui

            var builder = Host.CreateApplicationBuilder();
            builder.Configuration
                .AddJsonFile("Config/configValue.json", optional: false, reloadOnChange: true)
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);
            builder.Logging
                //.SetMinimumLevel(LogLevel.Trace) //default is info
                //.AddDebug() //this is default
                //.AddConsole() //this is default
                //.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning) //default is info
                .AddProvider(new FileLoggerProvider("D:/logs")) //write file logger
                .AddProvider(viewProvider)
                .AddSimpleConsole(o => o.IncludeScopes = true); //default is false
            builder.Services
                .AddSingleton(viewProvider) //add ui log provider
                .AddTransient<TransientClass>()
                .AddKeyedSingleton<SingletonClass>("key1")
                .AddKeyedSingleton<SingletonClass>("key2")
                .Configure<Appsettings>(builder.Configuration.GetSection("RunOptions")) //appsettings.json -> RunOptions
                .Configure<ConfigValue>(builder.Configuration.GetSection("ConfigValue")) //configValue.json -> ConfigValue
                .AddHostedService<RunClass>();
            using var host = builder.Build();
            await host.RunAsync(); //run IHostedService.StartAsync()

            Console.WriteLine("push Ctrl+C");
        }
    }
}
