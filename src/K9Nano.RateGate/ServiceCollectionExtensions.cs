using K9Nano.RateGate.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace K9Nano.RateGate
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddK9NanoRate(this IServiceCollection services)
        {
            services.AddSingleton<IRateManagerFactory, RateManagerFactory>()
                .AddSingleton<IRateStore, MemoryRateStore>()
                ;

            return services;
        }
    }
}
