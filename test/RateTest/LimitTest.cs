using System;
using System.Threading.Tasks;
using K9Nano.RateGate;
using K9Nano.RateGate.Internals;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace RateTest
{
    public class LimitTest
    {
        private readonly IServiceProvider _sp;

        public LimitTest()
        {
            var services = new ServiceCollection()
                .AddK9NanoRate()
                .Configure<RateLimitOptions>("test1", o =>
                {
                    o.Type = ERateLimitType.NaturalDay;
                    o.Limit = 2;
                })
                .Configure<RateLimitOptions>("test2", o =>
                {
                    o.Type = ERateLimitType.TimeSpan;
                    o.Limit = 2;
                    o.Period = TimeSpan.FromSeconds(2);
                });

            _sp = services.BuildServiceProvider();
        }

        [Fact]
        public void RunNaturalDay()
        {
            var manager = _sp.GetRequiredService<IRateManagerFactory>()
                .Create("test1");

            var result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Exceeded);

            var store = _sp.GetRequiredService<IRateStore>() as MemoryRateStore;

            var entity = store._data["test1"];
            entity.ExpiresAt = entity.ExpiresAt.AddDays(-1);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Exceeded);
        }

        [Fact]
        public async Task RunNaturalDayAsync()
        {
            var manager = _sp.GetRequiredService<IRateManagerFactory>()
                .Create("test1");

            var result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Success);

            result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Success);

            result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Exceeded);
        }

        [Fact]
        public async Task RunTimeSpan()
        {
            var manager = _sp.GetRequiredService<IRateManagerFactory>()
                .Create("test2");

            var result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Exceeded);

            await Task.Delay(2000);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Success);

            result = manager.Run(() => { });

            Assert.True(result.Status == ERateResultStatus.Exceeded);
        }

        [Fact]
        public async Task RunTimeSpanAsync()
        {
            var manager = _sp.GetRequiredService<IRateManagerFactory>()
                .Create("test2");

            var result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Success);

            result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Success);

            result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Exceeded);

            await Task.Delay(2000);

            result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Success);

            result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Success);

            result = await manager.RunAsync(c => Task.CompletedTask, default);

            Assert.True(result.Status == ERateResultStatus.Exceeded);
        }
    }
}
