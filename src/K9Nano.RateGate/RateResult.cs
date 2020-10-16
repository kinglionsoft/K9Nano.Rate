namespace K9Nano.RateGate
{
    public class RateResult
    {
        public ERateResultStatus Status { get; internal set; }
    }

    public class RateResult<T> : RateResult
    {
#pragma warning disable 8618 
        public T Result { get; internal set; }
#pragma warning restore 8618 
    }
}