using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace API.Middlewares
{
    public class RabbitMQLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnection? _connection;
        private readonly bool _rabbitAvailable;

        public RabbitMQLoggingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:HostName"],
                    Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                    UserName = configuration["RabbitMQ:UserName"],
                    Password = configuration["RabbitMQ:Password"]
                };

                _connection = factory.CreateConnectionAsync().Result;
                _rabbitAvailable = true;
            }
            catch
            {
                _connection = null;
                _rabbitAvailable = false;
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_rabbitAvailable)
            {
                await _next(context);
                return;
            }

            var start = DateTime.UtcNow;
            var username = context.User.FindFirst("name")?.Value ?? "unknown";

            await _next(context);

            var log = JsonSerializer.Serialize(new
            {
                User = username,
                Method = context.Request.Method,
                Path = context.Request.Path.ToString(),
                StatusCode = context.Response.StatusCode,
                ElapsedMs = (DateTime.UtcNow - start).TotalMilliseconds,
                Time = DateTime.UtcNow
            });

            try
            {
                using var channel = await _connection!.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "api-logs",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                var body = Encoding.UTF8.GetBytes(log);

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "api-logs",
                    body: body);
            }
            catch
            {
                // RabbitMQ недоступен
            }
        }
    }
}