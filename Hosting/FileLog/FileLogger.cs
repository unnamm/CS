using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.FileLog
{
    internal class FileLogger : ILogger
    {
        private static readonly AsyncLocal<Stack<object>> _scopeStack = new();

        private readonly string _categoryName;
        private readonly string _filePath;
        private readonly object _lock;

        public FileLogger(string categoryName, string filePath, object @lock)
        {
            _categoryName = categoryName.Contains('.') ? categoryName[(categoryName.LastIndexOf('.') + 1)..] : categoryName;
            _filePath = filePath;
            _lock = @lock;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            _scopeStack.Value ??= new Stack<object>();
            _scopeStack.Value.Push(state);
            return new ScopeDisposable(_scopeStack.Value);
        }

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

            var scopeInfo = ": ";
            if (_scopeStack.Value is { Count: > 0 })
                scopeInfo = " => " + string.Join(" => ", _scopeStack.Value) + ", ";

            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{levelStr}] {_categoryName}[{eventId.Id}]{scopeInfo}{formatter(state, exception)}";
            if (exception != null)
                line += Environment.NewLine + exception;

            lock (_lock)
            {
                File.AppendAllText(_filePath, line + Environment.NewLine);
            }
        }
    }
}
