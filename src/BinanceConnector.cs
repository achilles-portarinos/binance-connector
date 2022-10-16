using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net.Clients;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using moderndrummer.binance.Model;

namespace moderndrummer.binance
{
    public class BinanceConnector
    {
        private readonly BinanceClient _client;

        public BinanceConnector(string apiKey, string apiSecret)
        {
            _client = new BinanceClient(new BinanceClientOptions()
            {
                // Specify options for the client
                ApiCredentials = new ApiCredentials(apiKey, apiSecret)
            });
        }

        public async Task<IEnumerable<Earning>> GetTotalBalances()
        {
            var taskList = new List<Task<IEnumerable<Earning>>>()
            {
                GetSpotBalances(),
                GetCardBalances(),
                GetEarningsAsync(),
                GetVaultAsync()
            };

            var results = await Task.WhenAll(taskList);
            var items = results.SelectMany(i => i).ToList();

            // consolidate
            var totals = items.GroupBy(i => i.Asset)
                .Select(g => new Earning
                {
                    Asset = g.Key,
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderBy(i => i.Asset);

            return totals;
        }

        public async Task<IEnumerable<Earning>> GetSpotBalances()
        {
            var result = await _client.SpotApi.Account.GetUserAssetsAsync();

            var balances = result.Data
                .Where(i => i.Available > 0 || i.Freeze > 0 || i.Locked > 0)
                .Select(i => new Earning
                {
                    Asset = i.Asset,
                    Amount = i.Available + i.Locked
                });

            return balances;
        }

        public async Task<IEnumerable<Earning>> GetCardBalances()
        {
            var result = await _client.SpotApi.Account.GetFundingWalletAsync();

            var balances = result.Data
                .Where(i => i.Available > 0 || i.Freeze > 0 || i.Locked > 0)
                .Select(i => new Earning
                {
                    Asset = i.Asset,
                    Amount = i.Available + i.Locked
                });

            return balances;
        }

        public async Task<IEnumerable<Earning>> GetEarningsAsync()
        {
            var taskList = new List<Task<CryptoExchange.Net.Objects.WebCallResult<IEnumerable<Binance.Net.Objects.Models.Spot.Staking.BinanceStakingPosition>>>>()
            {
                _client.SpotApi.Trading.GetStakingPositionsAsync(Binance.Net.Enums.StakingProductType.LockedDeFi),
                _client.SpotApi.Trading.GetStakingPositionsAsync(Binance.Net.Enums.StakingProductType.FlexibleDeFi),
                _client.SpotApi.Trading.GetStakingPositionsAsync(Binance.Net.Enums.StakingProductType.Staking)
            };

            var results = await Task.WhenAll(taskList);
            var items = results.SelectMany(i => i.Data).ToList();

            var balances = items
                .Where(i => i.Quantity > 0)
                .Select(i => new Earning
                {
                    Asset = i.Asset,
                    Amount = i.Quantity
                });

            return balances;
        }

        public async Task<IEnumerable<Earning>> GetVaultAsync()
        {
            var items = await _client.GeneralApi.Savings.GetFlexibleProductPositionAsync();

            var balances = items.Data
                .Where(i => i.TotalQuantity > 0)
                .Select(i => new Earning
                {
                    Asset = i.Asset,
                    Amount = i.TotalQuantity
                });

            return balances;
        }
    }
}
