using System;
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

        public static async Task Main(string[] args)
        {
            ServiceCollection serviceCollection = new();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var binanceConnector = new BinanceConnector(Configuration["apiKey"], Configuration["apiSecret"]);
            var earnings = await binanceConnector.ParseEarnings();
            var balances = await binanceConnector.GetSpotBalances();

            // consolidate
            var totals = earnings.Union(balances).GroupBy(i => i.Asset)
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
