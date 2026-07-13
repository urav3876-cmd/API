using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Data;
using API.Services;
using API.Interfaces;
using Microsoft.OpenApi.Models;
using API.Middlewares;


//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// JWT �����
//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoidGVzdHVzZXIiLCJqdGkiOiIxMjM0NTY3OC0xMjM0LTEyMzQtMTIzNC0xMjM0NTY3ODlhYmMiLCJleHAiOjIwMDAwMDAwMDAsImF1ZCI6ImJldHRpbmctYXBpIiwiaXNzIjoiYmV0dGluZy1hcGkifQ.2SGv3FyCI7jIMfSJmGAsJagjH_-vCCiDPN7G6FsoyNo
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey is not configured");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });

            // AddAuthorization
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // ============================================================
            // ������ �������� � BettingDbContext (�� �������)
            // ============================================================
            builder.Services.AddDbContext<BettingDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ============================================================
            // SportsEvents �������� (�� �������)
            // ============================================================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("SportsEventsConnection")));

            // ============================================================
            // ����������� ������������ (�� �������)
            // ============================================================
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<ISportRepository, SportRepository>();
            builder.Services.AddScoped<IOutcomeRepository, OutcomeRepository>();
            builder.Services.AddScoped<IMarketRepository, MarketRepository>();

            //Services
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IBetService, BetService>();

            // ============================================================
            // ����������� �������� (�� �������)
            // ============================================================
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<ISportService, SportService>();
            builder.Services.AddScoped<IOutcomeService, OutcomeService>();
            builder.Services.AddScoped<IMarketService, MarketService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // ��������� �������� ����������� (�� �������)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var connectionString = dbContext.Database.GetConnectionString();
                Console.WriteLine($"���������� � ��: {connectionString}");

                try
                {
                    var canConnect = await dbContext.Database.CanConnectAsync();
                    Console.WriteLine($"����� ������������: {canConnect}");

                    if (canConnect)
                    {
                        var eventsCount = await dbContext.Events.CountAsync();
                        Console.WriteLine($"���������� ������� � ��: {eventsCount}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"������ �����������: {ex.Message}");
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //kafka
            var kafkaEnabled = builder.Configuration.GetValue<bool>("Kafka:Enabled");
            if (kafkaEnabled)
            {
                app.UseMiddleware<KafkaLoggingMiddleware>();
            }

            var rabbitEnabled = builder.Configuration.GetValue<bool>("RabbitMQ:Enabled");
            if (rabbitEnabled)
            {
                app.UseMiddleware<RabbitMQLoggingMiddleware>();
            }

            app.MapControllers();

            await app.RunAsync();
        }
    }
}