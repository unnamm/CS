using Hosting.Interface;
using Hosting.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Hosting.Base
{
    internal class ViewLoggerProvider : ILoggerProvider, ISupportExternalScope, IEntrySink
    {
        private readonly Action<Action> _uiInvoker;
        private IExternalScopeProvider? _scopeProvider;

        public ObservableCollection<LogEntry> Logs { get; } = [];
        public int MaxCount;

        public ViewLoggerProvider(Action<Action> invoker, int maxCount = 100)
        {
            _uiInvoker = invoker;
            MaxCount = maxCount;
        }

        public void Add(LogEntry log)
        {
            _uiInvoker(() =>
            {
                Logs.Add(log);
                if (Logs.Count > MaxCount)
                    Logs.RemoveAt(0);
            });
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;
        public ILogger CreateLogger(string categoryName) => new LoggerBase(categoryName, this, () => _scopeProvider);
        public void Dispose() { }
    }
}
