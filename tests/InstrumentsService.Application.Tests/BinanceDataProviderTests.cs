using InstrumentsService.Application.Services;
using InstrumentsService.Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace InstrumentsService.Application.Tests
{
    public class BinanceDataProviderTests
    {
        private readonly BinanceDataProvider _binanceDataProvider;
        private readonly HttpClient _httpClient;

        public BinanceDataProviderTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .AddEnvironmentVariables()
                .Build();
            
            var dataProviderConfig = new DataProviderConfiguration();
            configuration.GetSection("DataProviderConfiguration").Bind(dataProviderConfig);

            _httpClient = new HttpClient();
            _binanceDataProvider = new BinanceDataProvider(Options.Create(dataProviderConfig));
        }


        [Theory]
        [InlineData("BTCUSD")]
        [InlineData("USDJPY")]
        [InlineData("EURUSD")]
        public async Task GetPrice_ShouldReturnPrice(string symbol)
        {
            var price = await _binanceDataProvider.GetPrice(symbol);
            Assert.IsType<double>(price);
        }
    }
}
