using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ========================================
            // 1. ПОДКЛЮЧЕНИЕ К POSTGRESQL
            // ========================================
            // Регистрируем DbContext и говорим, что будем использовать PostgreSQL
            // Строка подключения берется из appsettings.json
            
            builder.Services.AddDbContext<EventDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // ========================================
            // 2. РЕГИСТРАЦИЯ НАШИХ СЕРВИСОВ
            // ========================================
            // AddScoped - создает один экземпляр сервиса на каждый HTTP-запрос
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<ISportRepository, SportRepository>();
            builder.Services.AddScoped<IOutcomeRepository, OutcomeRepository>();
            builder.Services.AddScoped<IMarketRepository, MarketRepository>();

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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}