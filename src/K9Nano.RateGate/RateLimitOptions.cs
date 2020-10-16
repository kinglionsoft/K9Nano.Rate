using System;

namespace K9Nano.RateGate
{
    public sealed class RateLimitOptions
    {
        public int Limit { get; set; }
        public ERateLimitType Type { get; set; }
        public TimeSpan Period { get; set; }
    }
}