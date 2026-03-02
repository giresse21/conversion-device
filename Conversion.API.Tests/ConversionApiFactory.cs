using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Conversion.API.Data;

namespace Conversion.API.Tests;

/// <summary>
/// Factory qui lance l'API en mémoire pour les tests d'intégration.
/// Remplace la base PostgreSQL par une base InMemory pour ne pas dépendre d'un serveur DB.
/// </summary>
public class ConversionApiFactory : WebApplicationFactory<Conversion.API.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // On utilise l'environnement "Testing" pour que Program.cs ne lance pas les migrations
        // (InMemory ne supporte pas les migrations PostgreSQL).
        builder.UseEnvironment("Testing");

        // Pas besoin de remplacer le DbContext ici : Program.cs enregistre déjà
        // UseInMemoryDatabase quand l'environnement est "Testing" (voir Program.cs).
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Création du schéma en mémoire (équivalent à EnsureCreated, pas de migrations).
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            // Données de test : 2 devises et 1 taux pour les tests d'intégration.
            if (!db.Currencies.Any())
            {
                db.Currencies.Add(new Conversion.API.Models.Currency { Code = "EUR", Name = "Euro" });
                db.Currencies.Add(new Conversion.API.Models.Currency { Code = "USD", Name = "Dollar" });
                db.SaveChanges();
                var eur = db.Currencies.First(c => c.Code == "EUR");
                var usd = db.Currencies.First(c => c.Code == "USD");
                db.CurrencyRates.Add(new Conversion.API.Models.CurrencyRate
                {
                    CurrencyFromId = eur.Id,
                    CurrencyToId = usd.Id,
                    Rate = 1.1m
                });
                db.SaveChanges();
            }
        }

        return host;
    }
}
