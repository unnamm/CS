using Hosting.Config;
using Hosting.Interface;
using Hosting.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Hosting.Base
{
    internal class ViewLoggerProvider : ILoggerProvider, ISupportExternalScope, IEntrySink
    {
        private Action<Action> _uiInvoker = action => action();
        private IExternalScopeProvider? _scopeProvider;

        public ObservableCollection<LogEntry> Logs { get; } = [];
        public int MaxCount;

        public ViewLoggerProvider(IOptions<Appsettings> options)
        {
            MaxCount = options.Value.LogMaxValue;
        }

        public void SetInvoker(Action<Action> invoker) => _uiInvoker = invoker;

        public void Add(LogEntry log)
        {
            _uiInvoker(() =>
            {
                Logs.Add(log);
                while (Logs.Count > MaxCount)
                    Logs.RemoveAt(0);
            });
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;
        public ILogger CreateLogger(string categoryName) => new LoggerBase(categoryName, this, () => _scopeProvider);
        public void Dispose() { }
    }
}
