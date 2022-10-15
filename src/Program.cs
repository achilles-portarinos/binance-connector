using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using moderndrummer.binance.Model;

namespace moderndrummer.binance
{
    public class Program
    {
        private static IConfigurationRoot Configuration { get; set; }

        public static async Task Main()
        {
            ServiceCollection serviceCollection = new();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var binanceConnector = new BinanceConnector(Configuration["apiKey"], Configuration["apiSecret"]);
            var taskList = new List<Task<IEnumerable<Earning>>>()
            {
                binanceConnector.GetSpotBalances(),
                binanceConnector.GetCardBalances(),
                binanceConnector.GetEarningsAsync(),
                binanceConnector.GetVaultAsync()
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

            foreach (var item in totals)
            {
                Console.WriteLine($"{item.Asset.ToMinStringLength(6)}: {Math.Round(item.Amount, 5).ToString().ToMinStringLength(12)}");
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            string settingsFile = "appsettings.json";
#if DEBUG
            settingsFile = "appsettings.development.json";
#endif
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile(settingsFile, false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(Configuration);
        }
    }
}
