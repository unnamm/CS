using Hosting.Config;
using Hosting.Interface;
using Hosting.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.Base
{
    internal class FileLoggerProvider : ILoggerProvider, ISupportExternalScope, IEntrySink
    {
        private readonly IOptionsMonitor<Appsettings> _options;
        private readonly object _lock = new();
        private IExternalScopeProvider? _scopeProvider;
        private DateTime _currentDate;
        private string? _filePath;

        public FileLoggerProvider(IOptionsMonitor<Appsettings> option)
        {
            _options = option;
        }

        private string GetFilePath()
        {
            var today = DateTime.Now.Date;
            if (_filePath == null || today != _currentDate)
            {
                var folderPath = _options.CurrentValue.LogFolderPath!;
                Directory.CreateDirectory(folderPath);

                _currentDate = today;
                _filePath = Path.Combine(folderPath, $"{today:yyyy-MM-dd}.txt");
            }

            return _filePath;
        }

        public void Add(LogEntry entry)
        {
            lock (_lock)
            {
                File.AppendAllText(GetFilePath(), entry.ToString());
            }
        }

        public ILogger CreateLogger(string categoryName) => new LoggerBase(categoryName, this, () => _scopeProvider);
        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;
        public void Dispose() { }
    }
}
