using System.Collections.Generic;
using Newtonsoft.Json;

namespace moderndrummer.binance.Model
{
    public class EarningsWrapper
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("messageDetail")]
        public object MessageDetail { get; set; }

        [JsonProperty("data")]
        public List<BinanceEarning> Data { get; set; } = new List<BinanceEarning>();

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}