using Hosting.Base;
using Hosting.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Hosting
{
    internal class RunClass : BackgroundService
    {
        private readonly SingletonClass _c1;
        private readonly SingletonClass _c2;
        private readonly TransientClass _t;
        private readonly ObservableCollection<LogEntry> _logs;
        private readonly ILogger<RunClass> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public RunClass(TransientClass t2, ViewLoggerProvider vp, ILogger<RunClass> logger, IHostApplicationLifetime lt,
            [FromKeyedServices("key1")] SingletonClass c1,
            [FromKeyedServices("key2")] SingletonClass c2)
        {
            _c1 = c1;
            _c2 = c2;
            _t = t2;

            _logs = vp.Logs;
            _logger = logger;
            _lifetime = lt;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("start");
            return base.StartAsync(cancellationToken); //run ExecuteAsync()
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //_logger.LogInformation("run..");
                    _c1.Print();
                    _c2.Print();
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
