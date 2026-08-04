using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting
{
    internal partial class SingletonClass
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "high speed log={data}")]
        public static partial void Run(ILogger logger, string data);

        private readonly int _data = new Random().Next();
        private readonly object _key;
        private readonly ILogger<SingletonClass> _log;
        private readonly TransientClass _t;

        public SingletonClass(ILogger<SingletonClass> log, TransientClass t, [ServiceKey] object key)
        {
            _log = log;
            _t = t;
            _key = key;
        }

        public void Print()
        {
            using (_log.BeginScope(_key.ToString()!))
            {
                _log.LogInformation("data={_data}", _data);
            }
        }

        public void Scope()
        {
            using (_log.BeginScope("ID: 4"))
            {
                _log.LogCritical(3, "LogCritical");
                _log.LogDebug("LogDebug");
                _log.LogError("LogError");
                _log.LogInformation("LogInformation");
                _log.LogTrace("LogTrace");
                _log.LogWarning("LogWarning");

                _log.Log(LogLevel.Critical, "Critical");
                _log.Log(LogLevel.Debug, "Debug");
                _log.Log(LogLevel.Error, "Error");
                _log.Log(LogLevel.Information, "Information");
                _log.Log(LogLevel.None, "None");
                _log.Log(LogLevel.Trace, "Trace");
                _log.Log(LogLevel.Warning, "Warning");

                _t.Print();
            }
        }

        public void LoggerMessage(string data) => Run(_log, data);
    }
}
