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
// ≈ƒ»Õ€…  ŒÕ“≈ —“
// ============================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================================================
// –≈√»—“–¿÷»ﬂ –≈œŒ«»“Œ–»≈¬
// ============================================================
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISportRepository, SportRepository>();
builder.Services.AddScoped<IOutcomeRepository, OutcomeRepository>();
builder.Services.AddScoped<IMarketRepository, MarketRepository>();

// ============================================================
// –≈√»—“–¿÷»ﬂ —≈–¬»—Œ¬
// ============================================================
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISportService, SportService>();
builder.Services.AddScoped<IOutcomeService, OutcomeService>();
builder.Services.AddScoped<IMarketService, MarketService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();