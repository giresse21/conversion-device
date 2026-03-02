using System.Net;
using System.Net.Http.Json;
using Conversion.API.DTOs.Currency;
using Xunit;

namespace Conversion.API.Tests.Controllers;

/// <summary>
/// Tests d'intégration du CurrenciesController.
/// On lance une vraie instance de l'API (en mémoire) et on envoie de vraies requêtes HTTP.
/// La base est celle seedée dans ConversionApiFactory (EUR, USD).
/// </summary>
public class CurrenciesControllerIntegrationTests : IClassFixture<ConversionApiFactory>
{
    private readonly HttpClient _client;

    public CurrenciesControllerIntegrationTests(ConversionApiFactory factory)
    {
        // HttpClient fourni par la factory pointe vers notre API (pas de vrai réseau).
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Retourne_Ok_Et_Une_Liste_De_Devises()
    {
        // Act : requête GET /api/currencies
        var response = await _client.GetAsync("/api/currencies");

        // Assert : statut 200 et corps interprété comme liste de CurrencyDto
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<CurrencyDto>>();
        Assert.NotNull(list);
        Assert.True(list.Count >= 2); // au moins EUR et USD (seed)
        Assert.Contains(list, c => c.Code == "EUR" && c.Name == "Euro");
        Assert.Contains(list, c => c.Code == "USD" && c.Name == "Dollar");
    }

    [Fact]
    public async Task GetById_Retourne_404_Pour_Id_Inexistant()
    {
        var response = await _client.GetAsync("/api/currencies/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Retourne_La_Devise_Pour_Un_Id_Valide()
    {
        // On récupère d'abord la liste pour avoir un id existant.
        var list = await _client.GetFromJsonAsync<List<CurrencyDto>>("/api/currencies");
        Assert.NotNull(list);
        var id = list[0].Id;

        var response = await _client.GetAsync($"/api/currencies/{id}");
        response.EnsureSuccessStatusCode();
        var currency = await response.Content.ReadFromJsonAsync<CurrencyDto>();
        Assert.NotNull(currency);
        Assert.Equal(id, currency.Id);
    }

    [Fact]
    public async Task Create_Retourne_201_Et_La_Devise_Creee()
    {
        var dto = new CreateCurrencyDto { Code = "GBP", Name = "Livre sterling" };
        var response = await _client.PostAsJsonAsync("/api/currencies", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<CurrencyDto>();
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("GBP", created.Code);
        Assert.Equal("Livre sterling", created.Name);
    }
}
