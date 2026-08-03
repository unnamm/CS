using Hosting.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.Abstract
{
    internal abstract class LoggerBase<T> : ILogger
    {
        protected readonly T _provider;

        private readonly string _categoryName;
        private readonly Func<IExternalScopeProvider?> _scopeProviderAccessor;

        public LoggerBase(string categoryName, T provider, Func<IExternalScopeProvider?> scopeProviderAccessor)
        {
            _categoryName = categoryName.Contains('.') ? categoryName[(categoryName.LastIndexOf('.') + 1)..] : categoryName;
            _provider = provider;
            _scopeProviderAccessor = scopeProviderAccessor;
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

            Write(entry);
        }

        protected abstract void Write(LogEntry entry);
    }
}
