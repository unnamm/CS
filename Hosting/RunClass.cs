using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting
{
    internal class RunClass : BackgroundService
    {
        private readonly SingletonClass _c;
        private readonly TransientClass _t;

        public RunClass(SingletonClass c1, TransientClass c2)
        {
            _c = c1;
            _t = c2;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _c.Print();
            _c.Scope();
            _c.LoggerMessage("high");
            _t.Print();
        }
    }
}
