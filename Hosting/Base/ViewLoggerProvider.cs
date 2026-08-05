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
        private readonly IOptionsMonitor<Appsettings> _options;

        public ObservableCollection<LogEntry> Logs { get; } = [];

        public ViewLoggerProvider(IOptionsMonitor<Appsettings> options)
        {
            _options = options;
        }

        public void SetInvoker(Action<Action> invoker) => _uiInvoker = invoker;

        public void Add(LogEntry log)
        {
            _uiInvoker(() =>
            {
                Logs.Add(log);
                while (Logs.Count > _options.CurrentValue.LogMaxValue)
                    Logs.RemoveAt(0);
            });
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;
        public ILogger CreateLogger(string categoryName) => new LoggerBase(categoryName, this, () => _scopeProvider);
        public void Dispose() { }
    }
}
