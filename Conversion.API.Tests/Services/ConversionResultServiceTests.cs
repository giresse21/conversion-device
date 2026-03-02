using Conversion.API.DTOs.Conversion;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Conversion.API.Services;
using Moq;
using Xunit;

namespace Conversion.API.Tests.Services;

/// <summary>
/// Tests unitaires du ConversionResultService.
/// On mocke les repositories (conversion results + currency rate) pour tester la logique de création.
/// </summary>
public class ConversionResultServiceTests
{
    private readonly Mock<IConversionResultRepository> _resultRepoMock;
    private readonly Mock<ICurrencyRateRepository> _rateRepoMock;
    private readonly ConversionResultService _service;

    public ConversionResultServiceTests()
    {
        _resultRepoMock = new Mock<IConversionResultRepository>();
        _rateRepoMock = new Mock<ICurrencyRateRepository>();
        _service = new ConversionResultService(_resultRepoMock.Object, _rateRepoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Leve_InvalidOperationException_Si_Pas_De_Taux()
    {
        // Si aucun taux n'existe pour la paire de devises, le service doit lever une exception.
        _rateRepoMock
            .Setup(r => r.GetRateAsync(1, 2))
            .ReturnsAsync((CurrencyRate?)null);

        var dto = new CreateConversionResultDto
        {
            CurrencyFromId = 1,
            CurrencyToId = 2,
            Value = 100m
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));

        // AddAsync ne doit pas être appelé car on a levé avant.
        _resultRepoMock.Verify(r => r.AddAsync(It.IsAny<ConversionResult>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Enregistre_Et_Retourne_Le_Resultat_Quand_Taux_Existe()
    {
        // On simule un taux EUR -> USD = 1.1
        var from = new Currency { Id = 1, Code = "EUR", Name = "Euro" };
        var to = new Currency { Id = 2, Code = "USD", Name = "Dollar" };
        var rateEntity = new CurrencyRate
        {
            Id = 1,
            CurrencyFromId = 1,
            CurrencyToId = 2,
            Rate = 1.1m,
            CurrencyFrom = from,
            CurrencyTo = to
        };
        _rateRepoMock.Setup(r => r.GetRateAsync(1, 2)).ReturnsAsync(rateEntity);

        var savedEntity = new ConversionResult
        {
            Id = 1,
            CurrencyFromId = 1,
            CurrencyToId = 2,
            Rate = 1.1m,
            Value = 100m,
            CreatedAt = DateTime.UtcNow,
            CurrencyFrom = from,
            CurrencyTo = to
        };
        _resultRepoMock
            .Setup(r => r.AddAsync(It.IsAny<ConversionResult>()))
            .ReturnsAsync(savedEntity);

        var dto = new CreateConversionResultDto
        {
            CurrencyFromId = 1,
            CurrencyToId = 2,
            Value = 100m
        };

        var result = await _service.CreateAsync(dto);

        Assert.Equal(1, result.Id);
        Assert.Equal("EUR", result.CurrencyFromCode);
        Assert.Equal("USD", result.CurrencyToCode);
        Assert.Equal(1.1m, result.Rate);
        Assert.Equal(100m, result.Value);
        _resultRepoMock.Verify(r => r.AddAsync(It.Is<ConversionResult>(e =>
            e.CurrencyFromId == 1 && e.CurrencyToId == 2 && e.Rate == 1.1m && e.Value == 100m)), Times.Once);
    }
}
