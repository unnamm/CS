using Hosting.Abstract;
using Hosting.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.ViewLog
{
    internal class ViewLogger : LoggerBase<ViewLoggerProvider>
    {
        public ViewLogger(string categoryName, ViewLoggerProvider provider, Func<IExternalScopeProvider?> scopeProviderAccessor)
            : base(categoryName, provider, scopeProviderAccessor) { }

        protected override void Write(LogEntry entry)
        {
            _provider.Add(entry);
        }
    }
}
