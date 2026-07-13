using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================================
// ЕДИНЫЙ КОНТЕКСТ
// ============================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SportsEventsConnection")));

// ============================================================
// РЕГИСТРАЦИЯ РЕПОЗИТОРИЕВ
// ============================================================
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISportRepository, SportRepository>();
builder.Services.AddScoped<IOutcomeRepository, OutcomeRepository>();
builder.Services.AddScoped<IMarketRepository, MarketRepository>();

// ============================================================
// РЕГИСТРАЦИЯ СЕРВИСОВ
// ============================================================
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISportService, SportService>();
builder.Services.AddScoped<IOutcomeService, OutcomeService>();
builder.Services.AddScoped<IMarketService, MarketService>();

var app = builder.Build();

// ВРЕМЕННАЯ ПРОВЕРКА ПОДКЛЮЧЕНИЯ
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var connectionString = dbContext.Database.GetConnectionString();
    Console.WriteLine($"Подключено к БД: {connectionString}");

    try
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        Console.WriteLine($"Можно подключиться: {canConnect}");

        if (canConnect)
        {
            var eventsCount = await dbContext.Events.CountAsync();
            Console.WriteLine($"Количество событий в БД: {eventsCount}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка подключения: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();