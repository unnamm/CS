using Hosting.Interface;
using Hosting.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.Base
{
    internal class LoggerBase : ILogger
    {
        private readonly string _categoryName;
        private readonly IEntrySink _provider;
        private readonly Func<IExternalScopeProvider?> _scopeProviderAccessor;

        public LoggerBase(string categoryName, IEntrySink provider, Func<IExternalScopeProvider?> scopeProviderAccessor)
        {
            _categoryName = categoryName.Contains('.') ? categoryName[(categoryName.LastIndexOf('.') + 1)..] : categoryName;
            _scopeProviderAccessor = scopeProviderAccessor;
            _provider = provider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _scopeProviderAccessor()?.Push(state);
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var entry = new LogEntry(_scopeProviderAccessor)
            {
                Timestamp = DateTime.Now,
                Level = logLevel,
                Category = _categoryName,
                EventId = eventId.Id,
                Message = formatter(state, exception),
                Exception = exception
            };

            _provider.Add(entry);
        }
    }
}
