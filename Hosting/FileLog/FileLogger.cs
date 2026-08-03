using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.FileLog
{
    internal class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly string _filePath;
        private readonly object _lock;
        private readonly Func<IExternalScopeProvider?> _scopeProviderAccessor;

        public FileLogger(string categoryName, string filePath, object @lock, Func<IExternalScopeProvider?> scopeProviderAccessor)
        {
            _categoryName = categoryName.Contains('.') ? categoryName[(categoryName.LastIndexOf('.') + 1)..] : categoryName;
            _filePath = filePath;
            _lock = @lock;
            _scopeProviderAccessor = scopeProviderAccessor;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _scopeProviderAccessor()?.Push(state);
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var levelStr = logLevel switch
            {
                LogLevel.Trace => "trac",
                LogLevel.Debug => "debu",
                LogLevel.Information => "info",
                LogLevel.Warning => "warn",
                LogLevel.Error => "fail",
                LogLevel.Critical => "crit",
                _ => "    "
            };

            var scopeParts = new List<string>();
            _scopeProviderAccessor()?.ForEachScope((scope, sb) => sb.Add(scope?.ToString() ?? ""), scopeParts);
            var scopeInfo = scopeParts.Count > 0 ? " => " + string.Join(" => ", scopeParts) : "";
            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{levelStr}] {_categoryName}[{eventId.Id}]: {formatter(state, exception)}{scopeInfo}";
            if (exception != null)
                line += Environment.NewLine + exception;

            lock (_lock)
            {
                File.AppendAllText(_filePath, line + Environment.NewLine);
            }
        }
    }
}
