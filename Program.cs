using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Services;
using API.Interfaces;

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
            builder.Services.AddDbContext<BettingDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddDbContext<EventDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // ========================================
            // 2. РЕГИСТРАЦИЯ НАШИХ СЕРВИСОВ
            // ========================================
            // AddScoped - создает один экземпляр сервиса на каждый HTTP-запрос
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IBetService, BetService>();
            builder.Services.AddScoped<IEventService, EventService>();

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
