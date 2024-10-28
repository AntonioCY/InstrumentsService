using InstrumentsService.Application.Interfaces;
using InstrumentsService.Domain.Configurations;
using InstrumentsService.Domain.Entities.Tiingo;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InstrumentsService.Application.Services;

public class TiingoDataProvider(HttpClient httpClient, IOptions<DataProviderConfiguration> configuration) : IDataProvider
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IOptions<DataProviderConfiguration> _configuration = configuration;

    public async Task<double> GetPrice(string instrument)
    {
        var baseUrl = _configuration.Value.Tiingo.BaseUrl;
        var token = _configuration.Value.Tiingo.Token;

        var response = await _httpClient.GetStringAsync($"{baseUrl}{instrument}&token={token}");
        var data = JsonConvert.DeserializeObject<List<TingoPriceResponse>>(response);

        if(data == null || data.Count == 0)
            return -1;

        return data!.First().AskPrice; // what exactly price is need to be returned - ask/bid?
    }
}
