using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.FileLog
{
    internal class FileLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly string _filePath;
        private readonly object _lock = new();
        private IExternalScopeProvider? _scopeProvider;

        public FileLoggerProvider(string filePath)
        {
            _filePath = filePath;
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, _filePath, _lock, () => _scopeProvider);
        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;
        public void Dispose() { }
    }
}
