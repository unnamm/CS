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
        private readonly string _folderPath;
        private readonly object _lock = new();
        private IExternalScopeProvider? _scopeProvider;
        private DateTime _currentDate;
        private string _filePath;

        public FileLoggerProvider(IOptions<Appsettings> option)
        {
            _folderPath = option.Value.LogFolderPath!;
            Directory.CreateDirectory(_folderPath);

            _currentDate = DateTime.Now.Date;
            _filePath = GetFilePath(_currentDate);
        }

        private string GetFilePath(DateTime date) => Path.Combine(_folderPath, $"{date:yyyy-MM-dd}.txt");

        public void Add(LogEntry entry)
        {
            lock (_lock)
            {
                var today = DateTime.Now.Date;
                if (today != _currentDate)
                {
                    _currentDate = today;
                    _filePath = GetFilePath(_currentDate);
                }

                File.AppendAllText(_filePath, entry.ToString());
            }
        }

        public ILogger CreateLogger(string categoryName) => new LoggerBase(categoryName, this, () => _scopeProvider);
        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;
        public void Dispose() { }
    }
}
