using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hosting.Model
{
    internal class LogEntry
    {
        public DateTime Timestamp { get; init; }
        public LogLevel Level { get; init; }
        public string Category { get; init; } = "";
        public EventId EventId { get; init; }
        public string Message { get; init; } = "";
        public string? Scope { get; }
        public Exception? Exception { get; init; }

        public LogEntry(Func<IExternalScopeProvider?> provider)
        {
            var scopeParts = new List<string>();
            provider()?.ForEachScope((scope, parts) => parts.Add(scope?.ToString() ?? ""), scopeParts);
            Scope = scopeParts.Count > 0 ? string.Join(" => ", scopeParts) : null;
        }

        public override string ToString()
        {
            var levelStr = Level switch
            {
                LogLevel.Trace => "trac",
                LogLevel.Debug => "debu",
                LogLevel.Information => "info",
                LogLevel.Warning => "warn",
                LogLevel.Error => "fail",
                LogLevel.Critical => "crit",
                _ => "    "
            };

            var line = $"{Timestamp:HH:mm:ss} [{levelStr}] {Category}[{EventId}]: {Message}{(Scope != null ? $" => {Scope}" : "")}{Environment.NewLine}";
            if (Exception != null)
                line += Exception + Environment.NewLine;

            return line;
        }
    }
}
