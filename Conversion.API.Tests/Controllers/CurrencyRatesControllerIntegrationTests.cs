using System.Net;
using System.Net.Http.Json;
using Conversion.API.DTOs.Currency;
using Conversion.API.DTOs.CurrencyRate;
using Xunit;

namespace Conversion.API.Tests.Controllers;

/// <summary>
/// Tests d'intégration du CurrencyRatesController.
/// La factory a seedé un taux EUR -> USD = 1.1, on peut le récupérer via GET from/to.
/// </summary>
public class CurrencyRatesControllerIntegrationTests : IClassFixture<ConversionApiFactory>
{
    private readonly HttpClient _client;

    public CurrencyRatesControllerIntegrationTests(ConversionApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Retourne_Ok_Et_Une_Liste()
    {
        var response = await _client.GetAsync("/api/currencyrates");
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<CurrencyRateDto>>();
        Assert.NotNull(list);
        Assert.True(list.Count >= 1); // au moins le taux EUR->USD seedé
    }

    [Fact]
    public async Task GetRate_Retourne_Le_Taux_Quand_La_Paire_Existe()
    {
        var currencies = await _client.GetFromJsonAsync<List<CurrencyDto>>("/api/currencies");
        Assert.NotNull(currencies);
        var eur = currencies.First(c => c.Code == "EUR");
        var usd = currencies.First(c => c.Code == "USD");

        var response = await _client.GetAsync($"/api/currencyrates/from/{eur.Id}/to/{usd.Id}");
        response.EnsureSuccessStatusCode();
        var rate = await response.Content.ReadFromJsonAsync<CurrencyRateDto>();
        Assert.NotNull(rate);
        Assert.Equal("EUR", rate.CurrencyFromCode);
        Assert.Equal("USD", rate.CurrencyToCode);
        Assert.Equal(1.1m, rate.Rate);
    }

    [Fact]
    public async Task GetRate_Retourne_404_Quand_Pas_De_Taux()
    {
        var currencies = await _client.GetFromJsonAsync<List<CurrencyDto>>("/api/currencies");
        Assert.NotNull(currencies);
        var eur = currencies.First(c => c.Code == "EUR");
        // Id inexistant pour la paire
        var response = await _client.GetAsync($"/api/currencyrates/from/{eur.Id}/to/99999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
