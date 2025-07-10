using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Xml.Linq;

namespace TaskForUzEx.Api.WebSockets;

public class WebSocketHandler
{
    private readonly ConcurrentDictionary<string, WebSocket> _clients = new();

    public async Task HandleWebSocket(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
            return;

        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var clientId = Guid.NewGuid().ToString();
        _clients.TryAdd(clientId, socket);

        try
        {
            var pingTimer = new Timer(async _ => await SendPing(socket), null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));

            while (socket.State == WebSocketState.Open)
            {
                var buffer = new byte[1024 * 4];
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _clients.TryRemove(clientId, out _);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    pingTimer.Dispose();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket error for client {clientId}: {ex.Message}");
            _clients.TryRemove(clientId, out _);
        }
        finally
        {
            if (socket.State != WebSocketState.Closed)
            {
                await socket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Closed by server", CancellationToken.None);
            }
        }
    }

    public async Task NotifyClients(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var client in _clients.ToList())
        {
            if (client.Value.State == WebSocketState.Open)
            {
                try
                {
                    await client.Value.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch
                {
                    _clients.TryRemove(client.Key, out _);
                }
            }
            else
            {
                _clients.TryRemove(client.Key, out _);
            }
        }
    }
    private async Task SendPing(WebSocket socket)
    {
        if (socket.State == WebSocketState.Open)
        {
            var pingMessage = Encoding.UTF8.GetBytes("PING");
            await socket.SendAsync(new ArraySegment<byte>(pingMessage), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
