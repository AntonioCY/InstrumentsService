using InstrumentsService.Application.Interfaces;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using InstrumentsService.Domain.Entities.Binance;
using InstrumentsService.Domain.Configurations;
using Microsoft.Extensions.Options;

namespace InstrumentsService.Application.Services;

public class BinanceDataProvider : IDataProvider
{
    private readonly ClientWebSocket _clientWebSocket;
    private readonly IOptions<DataProviderConfiguration> _configuration;
    public BinanceDataProvider(IOptions<DataProviderConfiguration> configuration)
    {
        _configuration = configuration;
        _clientWebSocket = new ClientWebSocket();
    }
    
    public async Task<double> GetPrice(string instrument)
    {
        await _clientWebSocket.ConnectAsync(new Uri("wss://stream.binance.com:443/ws/btcusdt"), CancellationToken.None);
        var subscribeMessage = new
        {
            method = "SUBSCRIBE",
            @params = new[] { $"{instrument}@aggTrade" },
            id = 1
        };
        var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(subscribeMessage));
        await _clientWebSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);

        var buffer = new byte[1024 * 4];
        var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var response = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var data = JsonConvert.DeserializeObject<BinancePriceResponse>(response);
        
        // Binance provider is need to be researched and completed, could be task for the TODO
        if(data!.Result == null)
        {
            return -1;
        }

        return data!.Result!.Price;
    }
}
