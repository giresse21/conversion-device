using System.Net;
using System.Net.Http.Json;
using Conversion.API.DTOs.Conversion;
using Conversion.API.DTOs.Currency;
using Xunit;

namespace Conversion.API.Tests.Controllers;

/// <summary>
/// Tests d'intégration du ConversionResultsController.
/// On utilise l'API réelle avec la base en mémoire (seed : EUR, USD, taux EUR->USD = 1.1).
/// </summary>
public class ConversionResultsControllerIntegrationTests : IClassFixture<ConversionApiFactory>
{
    private readonly HttpClient _client;

    public ConversionResultsControllerIntegrationTests(ConversionApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Retourne_Ok_Et_Une_Liste()
    {
        var response = await _client.GetAsync("/api/conversionresults");
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<ConversionResultDto>>();
        Assert.NotNull(list);
        // Peut être vide au départ ou contenir des résultats d'autres tests (base partagée).
    }

    [Fact]
    public async Task Create_Retourne_201_Et_Le_Resultat_Quand_Taux_Existe()
    {
        // On récupère les ids des devises seedées (EUR, USD).
        var currencies = await _client.GetFromJsonAsync<List<CurrencyDto>>("/api/currencies");
        Assert.NotNull(currencies);
        var eur = currencies.First(c => c.Code == "EUR");
        var usd = currencies.First(c => c.Code == "USD");

        var dto = new CreateConversionResultDto
        {
            CurrencyFromId = eur.Id,
            CurrencyToId = usd.Id,
            Value = 100m
        };

        var response = await _client.PostAsJsonAsync("/api/conversionresults", dto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ConversionResultDto>();
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("EUR", result.CurrencyFromCode);
        Assert.Equal("USD", result.CurrencyToCode);
        Assert.Equal(1.1m, result.Rate);
        Assert.Equal(100m, result.Value);
    }

    [Fact]
    public async Task Create_Retourne_400_Quand_Pas_De_Taux_Pour_La_Paire()
    {
        // On crée deux devises sans taux entre elles.
        var eur = (await _client.GetFromJsonAsync<List<CurrencyDto>>("/api/currencies"))!.First(c => c.Code == "EUR");
        var gbpResponse = await _client.PostAsJsonAsync("/api/currencies", new CreateCurrencyDto { Code = "XAU", Name = "Or" });
        gbpResponse.EnsureSuccessStatusCode();
        var xau = await gbpResponse.Content.ReadFromJsonAsync<CurrencyDto>();
        Assert.NotNull(xau);

        // Aucun taux EUR -> XAU n'existe.
        var dto = new CreateConversionResultDto
        {
            CurrencyFromId = eur.Id,
            CurrencyToId = xau.Id,
            Value = 100m
        };

        var response = await _client.PostAsJsonAsync("/api/conversionresults", dto);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_Retourne_404_Pour_Id_Inexistant()
    {
        var response = await _client.GetAsync("/api/conversionresults/99999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
