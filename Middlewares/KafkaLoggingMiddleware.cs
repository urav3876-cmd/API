using System.Text.Json;
using Confluent.Kafka;

namespace API.Middlewares
{
    public class KafkaLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaLoggingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                Acks = Acks.None,          // не ждём подтверждения от брокера
                LingerMs = 1               // до 1мс на сбор батча
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _topic = configuration["Kafka:Topic"] ?? "api-logs";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var start = DateTime.UtcNow;
            var username = context.User.FindFirst("name")?.Value ?? "unknown";

            // Request log — не ждём
            _producer.Produce(_topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(new
                {
                    Type = "Request",
                    User = username,
                    Method = context.Request.Method,
                    Path = context.Request.Path.ToString(),
                    Time = start
                })
            });

            await _next(context);

            // Response log — не ждём
            _producer.Produce(_topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(new
                {
                    Type = "Response",
                    User = username,
                    Method = context.Request.Method,
                    Path = context.Request.Path.ToString(),
                    StatusCode = context.Response.StatusCode,
                    ElapsedMs = (DateTime.UtcNow - start).TotalMilliseconds,
                    Time = DateTime.UtcNow
                })
            });
        }
    }
}