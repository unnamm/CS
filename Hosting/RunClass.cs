using Hosting.Base;
using Hosting.Model;
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
        private readonly SingletonClass _c;
        private readonly TransientClass _t;
        private readonly ObservableCollection<LogEntry> _logs;
        private readonly ILogger<RunClass> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public RunClass(SingletonClass c1, TransientClass c2, ViewLoggerProvider vp, ILogger<RunClass> logger, IHostApplicationLifetime lt)
        {
            _c = c1;
            _t = c2;

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
                    _logger.LogInformation("run..");

                    await Task.Delay(500, stoppingToken);
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
