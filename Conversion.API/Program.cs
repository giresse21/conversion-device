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


// DbContext : InMemory en environnement Testing (tests d'intégration), sinon PostgreSQL
if (builder.Environment.IsEnvironment("Testing"))
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("ConversionTestDb"));
else
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

// Pas de redirection HTTPS en Development ni en Testing (évite le warning dans les tests).
if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowReactApp");

app.MapControllers();


// Applique les migrations automatiquement (sauf en environnement Testing pour les tests d'intégration)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!app.Environment.IsEnvironment("Testing"))
        db.Database.Migrate();
}

app.Run();
