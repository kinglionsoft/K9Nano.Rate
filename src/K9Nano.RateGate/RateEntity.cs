using System;

namespace K9Nano.RateGate
{
    [Serializable]
    public class RateEntity
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}