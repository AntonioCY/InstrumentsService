using InstrumentsService.Domain.Constants;
using Microsoft.Extensions.Configuration;
using InstrumentsService.Domain.Entities.Tiingo.WS;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace InstrumentsService.Application.Handlers;

public static class WebSocketHandler
{
    private static readonly List<WebSocket> _sockets = [];

    public static async Task HandleWebSocketAsync(WebSocket webSocket)
    {
        _sockets.Add(webSocket);

        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        _sockets.Remove(webSocket);
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }

    public static async Task BroadcastMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    public static async Task SubscribeToExternalWebSocket(IConfiguration configuration)
    {
        string wssBaseUrl = configuration.GetSection("DataProviderConfiguration:Tiingo:WssBaseUrl").Value!;
        string token = configuration.GetSection("DataProviderConfiguration:Tiingo:Token").Value!;
        string broadcastInterval = configuration.GetSection("DataProviderConfiguration:Tiingo:BroadcastInterval").Value!;

        if (!int.TryParse(broadcastInterval, out int broadcastInt))
        {
            return;
        }

        using ClientWebSocket webSocket = new();
        await webSocket.ConnectAsync(new Uri(wssBaseUrl), CancellationToken.None);
        var buffer = new byte[1024 * 4];

        var subscribeMessage = new
        {
            eventName = "subscribe",
            authorization = token,
            eventData = new
            {
                thresholdLevel = 5,
                tickers = InstrumentsConstants.Symbols.ToArray()
            },
            id = 1
        };

        var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(subscribeMessage));
        await webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string response = Encoding.UTF8.GetString(buffer, 0, result.Count);

            var baseData = JsonConvert.DeserializeObject<TiingoWSBaseResponse>(response);

            if (baseData != null && baseData.MessageType == "A")
            {
                // Broadcast the received message to all connected clients

                var data = JsonConvert.DeserializeObject<TiingoWSResponse>(response);

                if (data?.MessageType == "A")
                {
                    await BroadcastMessageAsync(response);
                }

                await Task.Delay(broadcastInt);
            }
        }
    }
}
