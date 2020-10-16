namespace K9Nano.RateGate
{
    public interface IRateStore
    {
        bool TryIncrement(string name, RateLimitOptions options);
    }
}