using System;

namespace moderndrummer.binance.Model
{
    public class Earning
    {
        public string Asset { get; set; }
        public decimal Amount { get; set; }
        public DateTime? EarliestCreatedAt { get; set; }
    }
}