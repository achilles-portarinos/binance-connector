using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Clients;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using moderndrummer.binance.Model;
using Newtonsoft.Json;

namespace moderndrummer.binance
{
    public class BinanceConnector
    {
        private readonly string apiKey;
        private readonly string apiSecret;

        public BinanceConnector(string apiKey, string apiSecret)
        {
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        public async Task<IEnumerable<Earning>> ParseEarnings()
        {
            var json = await File.ReadAllTextAsync("earn.json");

            var earnData = JsonConvert.DeserializeObject<EarningsWrapper>(json);

            var earnings = earnData.Data;

            var calculated = earnings.GroupBy(x => x.Asset)
                .Select(g => new Earning
                {
                    Asset = g.Key,
                    Amount = g.Sum(x => x.Amount),
                    EarliestCreatedAt = g.Min(x => x.CreateTimestamp.ToDateTime()),
                });

            return calculated;
        }

        public async Task<IEnumerable<Earning>> GetSpotBalances()
        {
            var client = new BinanceClient(new BinanceClientOptions()
            {
                // Specify options for the client
                ApiCredentials = new ApiCredentials(apiKey, apiSecret)
            });

            var result = await client.SpotApi.Account.GetUserAssetsAsync();

            var balances = result.Data
                .Where(i => i.Available > 0 || i.Freeze > 0 || i.Locked > 0)
                .Select(i => new Earning
                {
                    Asset = i.Asset,
                    Amount = i.Available + i.Locked
                });

            return balances;
        }

    }
}
