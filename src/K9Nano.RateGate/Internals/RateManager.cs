using System;
using System.Threading;
using System.Threading.Tasks;

namespace K9Nano.RateGate.Internals
{
    internal class RateManager : IRateManager
    {
        private readonly string _name;
        private readonly IRateStore _store;

        public RateManager(string name, IRateStore store)
        {
            _name = name;
            _store = store;
        }

        public RateResult Run(Action worker)
        {
            var result = new RateResult();
            if (_store.TryIncrement(_name))
            {
                worker();
            }
            else
            {
                result.Status = ERateResultStatus.Exceeded;
            }
            return result;
        }

        public RateResult<T> Run<T>(Func<T> worker)
        {
            var result = new RateResult<T>();
            if (_store.TryIncrement(_name))
            {
                result.Result = worker();
            }
            else
            {
                result.Status = ERateResultStatus.Exceeded;
            }
            return result;
        }

        public async Task<RateResult> RunAsync(Func<CancellationToken, Task> worker, CancellationToken cancellation)
        {
            var result = new RateResult();
            if (_store.TryIncrement(_name))
            {
               await worker(cancellation);
            }
            else
            {
                result.Status = ERateResultStatus.Exceeded;
            }
            return result;
        }

        public async Task<RateResult<T>> RunAsync<T>(Func<CancellationToken, Task<T>> worker, CancellationToken cancellation)
        {
            var result = new RateResult<T>();
            if (_store.TryIncrement(_name))
            {
                result.Result = await worker(cancellation);
            }
            else
            {
                result.Status = ERateResultStatus.Exceeded;
            }

            return result;
        }
    }
}