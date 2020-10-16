using System;
using System.Threading;
using System.Threading.Tasks;

namespace K9Nano.RateGate
{
    public interface IRateManager
    {
        RateResult Run(Action worker);

        RateResult<T> Run<T>(Func<T> worker);

        Task<RateResult> RunAsync(Func<CancellationToken, Task> worker, CancellationToken cancellation);

        Task<RateResult<T>> RunAsync<T>(Func<CancellationToken, Task<T>> worker, CancellationToken cancellation);
    }
}