using Conversion.API.Data;
using Conversion.API.Repositories;
using Conversion.API.Repositories.Interfaces;
using Conversion.API.Services;
using Conversion.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();


// CORS : autorise les requêtes depuis le frontend React en développement
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3001") // URL du frontend React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Repositories
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IConversionResultRepository, ConversionResultRepository>();
builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();

// Services
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IConversionResultService, ConversionResultService>();
builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowReactApp");

app.MapControllers();


// Applique les migrations automatiquement
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
