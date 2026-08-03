using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting
{
    internal class TransientClass
    {
        private readonly ILogger<TransientClass> _log;
        private readonly int _data = new Random().Next();

        public TransientClass(ILogger<TransientClass> log)
        {
            _log = log;
        }
        public void Print() => _log.LogInformation("data={_data}", _data);
    }
}
