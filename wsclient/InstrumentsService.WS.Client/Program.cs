using System.Net.WebSockets;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        using ClientWebSocket webSocket = new();
        await webSocket.ConnectAsync(new Uri("ws://localhost:5132/ws"), CancellationToken.None);
        Console.WriteLine("Connected!");

        var receiveBuffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
            string message = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
            Console.WriteLine($"Received: {message}");
        }
    }
}

