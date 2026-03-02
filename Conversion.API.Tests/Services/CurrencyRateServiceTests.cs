using Conversion.API.DTOs.CurrencyRate;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Conversion.API.Services;
using Moq;
using Xunit;

namespace Conversion.API.Tests.Services;

/// <summary>
/// Tests unitaires du CurrencyRateService.
/// On mocke ICurrencyRateRepository pour vérifier le mapping et les appels.
/// </summary>
public class CurrencyRateServiceTests
{
    private readonly Mock<ICurrencyRateRepository> _repositoryMock;
    private readonly CurrencyRateService _service;

    public CurrencyRateServiceTests()
    {
        _repositoryMock = new Mock<ICurrencyRateRepository>();
        _service = new CurrencyRateService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetRateAsync_Retourne_Null_Si_Pas_De_Taux()
    {
        _repositoryMock.Setup(r => r.GetRateAsync(1, 2)).ReturnsAsync((CurrencyRate?)null);

        var result = await _service.GetRateAsync(1, 2);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetRateAsync_Retourne_Le_Taux_En_DTO()
    {
        var from = new Currency { Id = 1, Code = "EUR", Name = "Euro" };
        var to = new Currency { Id = 2, Code = "USD", Name = "Dollar" };
        var entity = new CurrencyRate
        {
            Id = 1,
            CurrencyFromId = 1,
            CurrencyToId = 2,
            Rate = 1.1m,
            CurrencyFrom = from,
            CurrencyTo = to
        };
        _repositoryMock.Setup(r => r.GetRateAsync(1, 2)).ReturnsAsync(entity);

        var result = await _service.GetRateAsync(1, 2);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("EUR", result.CurrencyFromCode);
        Assert.Equal("USD", result.CurrencyToCode);
        Assert.Equal(1.1m, result.Rate);
    }

    [Fact]
    public async Task CreateAsync_Enregistre_Et_Retourne_Le_Taux()
    {
        // GetByIdAsync est utilisé après Add pour récupérer les navigations (codes).
        var from = new Currency { Id = 1, Code = "EUR", Name = "Euro" };
        var to = new Currency { Id = 2, Code = "USD", Name = "Dollar" };
        var added = new CurrencyRate
        {
            Id = 1,
            CurrencyFromId = 1,
            CurrencyToId = 2,
            Rate = 1.2m,
            CurrencyFrom = from,
            CurrencyTo = to
        };
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<CurrencyRate>())).ReturnsAsync(added);
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(added);

        var dto = new CreateCurrencyRateDto
        {
            CurrencyFromId = 1,
            CurrencyToId = 2,
            Rate = 1.2m
        };

        var result = await _service.CreateAsync(dto);

        Assert.Equal(1, result.Id);
        Assert.Equal(1.2m, result.Rate);
        Assert.Equal("EUR", result.CurrencyFromCode);
        Assert.Equal("USD", result.CurrencyToCode);
    }
}
