using System.Net.WebSockets;
using System.Text;

namespace InstrumentsService.Application.Services
{
    public class WebSocketManager
    {
        private readonly Dictionary<string, List<WebSocket>> _subscriptions = new();

        public async Task Subscribe(string instrument, WebSocket socket)
        {
            if (!_subscriptions.ContainsKey(instrument))
            {
                _subscriptions[instrument] = new List<WebSocket>();
            }
            _subscriptions[instrument].Add(socket);

            // Handle incoming messages and broadcast updates
            await HandleWebSocket(socket, instrument);
        }

        private async Task HandleWebSocket(WebSocket socket, string instrument)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    _subscriptions[instrument].Remove(socket);
                }
            }
        }

        public async Task BroadcastPriceUpdate(string instrument, decimal price)
        {
            if (_subscriptions.ContainsKey(instrument))
            {
                var message = Encoding.UTF8.GetBytes(price.ToString());
                var tasks = _subscriptions[instrument].Select(socket => socket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None));
                await Task.WhenAll(tasks);
            }
        }
    }
}
