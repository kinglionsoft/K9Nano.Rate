namespace K9Nano.RateGate
{
    public interface IRateManagerFactory
    {
        IRateManager Create(string name);
    }
}