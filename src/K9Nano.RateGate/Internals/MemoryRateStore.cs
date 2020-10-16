using System.Collections.Generic;

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

        public MemoryRateStore()
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