using System;
using Microsoft.Extensions.Options;

namespace K9Nano.RateGate
{
    public abstract class RateStoreBase: IRateStore
    {
        protected readonly IOptionsMonitor<RateLimitOptions> OptionsMonitor;

        protected RateStoreBase(IOptionsMonitor<RateLimitOptions> optionsMonitor)
        {
            OptionsMonitor = optionsMonitor;
        }
        
        public virtual bool TryIncrement(string name)
        {
            var options = OptionsMonitor.Get(name);
            if (options == null)
            {
                throw new ArgumentException($"No RateLimitOptions named {name} found");
            }

            var entity = Get(name);

            if (entity == null)
            {
                entity = new RateEntity
                {
                    Name = name,
                    Count = 1,
                    ExpiresAt = CreateExpireTime(options)
                };
                Save(entity);
                return true;
            }

            if (entity.ExpiresAt < DateTime.Now)
            {
                entity.Count = 1;
                entity.ExpiresAt = CreateExpireTime(options);
                Save(entity);
                return true;
            }

            if (entity.Count < options.Limit)
            {
                entity.Count++;
                Save(entity);
                return true;
            }

            return false;
        }

        protected virtual DateTime CreateExpireTime(RateLimitOptions options)
        {
            switch (options.Type)
            {
                case ERateLimitType.NaturalDay:
                    return DateTime.Today.AddDays(1);
                case ERateLimitType.TimeSpan:
                    return DateTime.Now.Add(options.Period);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract void Save(RateEntity entity);

        protected abstract RateEntity? Get(string name);
    }
}