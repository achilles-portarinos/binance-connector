using Newtonsoft.Json;

namespace moderndrummer.binance.Model
{
    public class BinanceEarning
    {
        [JsonProperty("positionId")]
        public string PositionId { get; set; }

        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("amountBTC")]
        public object AmountBTC { get; set; }

        [JsonProperty("interest")]
        public string Interest { get; set; }

        [JsonProperty("interestBTC")]
        public object InterestBTC { get; set; }

        [JsonProperty("unpayInterest")]
        public string UnpayInterest { get; set; }

        [JsonProperty("interestType")]
        public string InterestType { get; set; }

        [JsonProperty("extraInterestAsset")]
        public object ExtraInterestAsset { get; set; }

        [JsonProperty("extraInterestAmount")]
        public object ExtraInterestAmount { get; set; }

        [JsonProperty("gross")]
        public object Gross { get; set; }

        [JsonProperty("grossBTC")]
        public object GrossBTC { get; set; }

        [JsonProperty("latePayInterest")]
        public object LatePayInterest { get; set; }

        [JsonProperty("purchaseTime")]
        public string PurchaseTime { get; set; }

        [JsonProperty("nextInterestPayDate")]
        public string NextInterestPayDate { get; set; }

        [JsonProperty("nextInterestPay")]
        public string NextInterestPay { get; set; }

        [JsonProperty("payInterestPeriod")]
        public string PayInterestPeriod { get; set; }

        [JsonProperty("accrualDays")]
        public string AccrualDays { get; set; }

        [JsonProperty("redeemDate")]
        public string RedeemDate { get; set; }

        [JsonProperty("endTime")]
        public object EndTime { get; set; }

        [JsonProperty("interestRate")]
        public string InterestRate { get; set; }

        [JsonProperty("extraInterestRate")]
        public object ExtraInterestRate { get; set; }

        [JsonProperty("canTransfer")]
        public bool CanTransfer { get; set; }

        [JsonProperty("canRedeemEarly")]
        public bool CanRedeemEarly { get; set; }

        [JsonProperty("redeemAmountEarly")]
        public string RedeemAmountEarly { get; set; }

        [JsonProperty("redeemPeriod")]
        public string RedeemPeriod { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createTimestamp")]
        public string CreateTimestamp { get; set; }

        [JsonProperty("deliverDate")]
        public string DeliverDate { get; set; }
    }
}