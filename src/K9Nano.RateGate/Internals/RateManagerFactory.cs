using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace K9Nano.RateGate.Internals
{
    internal class RateManagerFactory : IRateManagerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionsMonitor<RateLimitOptions> _optionsMonitor;
        private readonly ConcurrentDictionary<string, IRateManager> _cache;

        public RateManagerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<RateLimitOptions>>();

            _cache = new ConcurrentDictionary<string, IRateManager>();
        }

        public IRateManager Create(string name)
        {
            return _cache.GetOrAdd(name, key =>
            {
                var options = _optionsMonitor.Get(name);
                if (options == null)
                {
                    throw new ArgumentException($"No RateLimitOptions named {name} found");
                }

                var store = _serviceProvider.GetRequiredService<IRateStore>();
                IRateManager manager = new RateManager(name, options, store);
                return manager;
            });
        }

        public IRateManager Create(string name, RateLimitOptions options)
        {
            return _cache.GetOrAdd(name, key =>
            {
                var store = _serviceProvider.GetRequiredService<IRateStore>();
                IRateManager manager = new RateManager(name, options, store);
                return manager;
            });
        }
    }
}