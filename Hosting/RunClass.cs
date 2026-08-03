using Hosting.Model;
using Hosting.ViewLog;
using Microsoft.Extensions.Hosting;
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

        public RunClass(SingletonClass c1, TransientClass c2, ViewLoggerProvider vp)
        {
            _c = c1;
            _t = c2;

            _logs = vp.Logs;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _c.Print();
            _c.Scope();
            _c.LoggerMessage("high");
            _t.Print();

            var count = _logs.Count;
        }
    }
}
