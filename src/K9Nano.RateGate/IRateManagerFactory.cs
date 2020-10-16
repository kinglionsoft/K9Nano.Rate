namespace K9Nano.RateGate
{
    public interface IRateManagerFactory
    {
        IRateManager Create(string name);

        IRateManager Create(string name, RateLimitOptions options);
    }
}