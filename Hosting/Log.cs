using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting
{
    public class Log
    {
        public static void Sample()
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Logging.AddSimpleConsole(o => o.IncludeScopes = true); //show console scope
            builder.Services
                .AddSingleton<C1>()
                .AddTransient<C2>();
            using var host = builder.Build();

            var c1 = host.Services.GetRequiredService<C1>();
            c1.Print();
            var c1_2 = host.Services.GetRequiredService<C1>();
            c1_2.Print();
            c1_2.Scope();

            var c2_1 = host.Services.GetRequiredService<C2>();
            c2_1.Print();
            var c2_2 = host.Services.GetRequiredService<C2>();
            c2_2.Print();
        }
    }

    public partial class C1
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "init={data}")]
        public static partial void Run(ILogger logger, int data); //high speed

        private ILogger<C1> _log;
        private readonly int _data;

        public C1(ILogger<C1> log)
        {
            _log = log;
            _data = new Random().Next();

            Run(_log, _data);
        }

        public void Print()
        {
            _log.LogInformation("print={_data}", _data);
        }

        public void Scope()
        {
            using (_log.BeginScope("scope={data}", _data))
            {
                _log.LogInformation("scope={data}", _data);
                _log.LogInformation("scope");
            }
        }
    }

    public class C2
    {
        private ILogger<C2> _log;
        private readonly int _data;

        public C2(ILogger<C2> log)
        {
            _log = log;
            _data = new Random().Next();
        }
        public void Print()
        {
            _log.LogInformation("print={_data}", _data);
        }
    }
}
