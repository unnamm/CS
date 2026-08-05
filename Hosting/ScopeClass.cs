using Microsoft.Extensions.Logging;
using System;

namespace Hosting
{
    internal class ScopeClass
    {
        private readonly ILogger<ScopeClass> _logger;
        private readonly Guid _id = Guid.NewGuid();

        public ScopeClass(ILogger<ScopeClass> logger)
        {
            _logger = logger;
        }

        public void Print() => _logger.LogInformation("scope id={Id}", _id);
    }
}
