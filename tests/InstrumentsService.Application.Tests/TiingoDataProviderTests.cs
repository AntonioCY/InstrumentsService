using InstrumentsService.Application.Services;
using InstrumentsService.Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace InstrumentsService.Application.Tests
{
    public class TiingoDataProviderTests
    {
        private readonly TiingoDataProvider _tiingoDataProvider;
        private readonly HttpClient _httpClient;

        public TiingoDataProviderTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .AddEnvironmentVariables()
                .Build();
            
            var dataProviderConfig = new DataProviderConfiguration();
            configuration.GetSection("DataProviderConfiguration").Bind(dataProviderConfig);

            _httpClient = new HttpClient();
            _tiingoDataProvider = new TiingoDataProvider(_httpClient, Options.Create(dataProviderConfig));
        }


        [Theory]
        [InlineData("BTCUSD")]
        [InlineData("USDJPY")]
        [InlineData("EURUSD")]
        public async Task GetPrice_ShouldReturnPrice(string symbol)
        {
            var price = await _tiingoDataProvider.GetPrice(symbol);
            Assert.IsType<double>(price);
        }
    }
}
