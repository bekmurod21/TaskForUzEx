using TaskForUzEx.Api.WebSockets;

namespace TaskForUzEx.Api.Middlewares;

public class WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
{
    private readonly RequestDelegate _next = next;
    private readonly WebSocketHandler _webSocketHandler = webSocketHandler;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws")
        {
            await _webSocketHandler.HandleWebSocket(context);
        }
        else
        {
            await _next(context);
        }
    }
}