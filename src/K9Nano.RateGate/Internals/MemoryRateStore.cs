using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace K9Nano.RateGate.Internals
{
#if DEBUG
    public class MemoryRateStore : RateStoreBase
    {
        public readonly Dictionary<string, RateEntity> _data;
#else
    internal class MemoryRateStore : RateStoreBase
    {
        private readonly Dictionary<string, RateEntity> _data;
#endif

        public MemoryRateStore(IOptionsMonitor<RateLimitOptions> optionsMonitor) : base(optionsMonitor)
        {
            _data = new Dictionary<string, RateEntity>();
        }

        protected override void Save(RateEntity entity)
        {
            _data[entity.Name] = entity;
        }

        protected override RateEntity? Get(string name)
        {
            return _data.ContainsKey(name) ? _data[name] : null;
        }
    }
}