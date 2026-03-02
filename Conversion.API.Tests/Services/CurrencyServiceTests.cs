using Conversion.API.DTOs.Currency;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Conversion.API.Services;
using Moq;
using Xunit;

namespace Conversion.API.Tests.Services;

/// <summary>
/// Tests unitaires du CurrencyService.
/// On mocke ICurrencyRepository pour isoler la logique métier du service.
/// </summary>
public class CurrencyServiceTests
{
    private readonly Mock<ICurrencyRepository> _repositoryMock;
    private readonly CurrencyService _service;

    public CurrencyServiceTests()
    {
        // Mock = faux objet qui simule le repository (pas de vraie base de données).
        _repositoryMock = new Mock<ICurrencyRepository>();
        _service = new CurrencyService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_Retourne_La_Liste_Des_Devises()
    {
        // Arrange : on définit ce que le mock doit retourner quand GetAllAsync est appelé.
        var entities = new List<Currency>
        {
            new() { Id = 1, Code = "EUR", Name = "Euro" },
            new() { Id = 2, Code = "USD", Name = "Dollar" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

        // Act : on appelle la méthode du service.
        var result = await _service.GetAllAsync();

        // Assert : on vérifie le résultat (mapping vers DTO).
        Assert.Equal(2, result.Count);
        Assert.Equal("EUR", result[0].Code);
        Assert.Equal("Euro", result[0].Name);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("USD", result[1].Code);
    }

    [Fact]
    public async Task GetByIdAsync_Retourne_Null_Si_Devise_Inexistante()
    {
        // Le repository retourne null pour un id inconnu.
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Currency?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_Retourne_La_Devise_Si_Existante()
    {
        var entity = new Currency { Id = 1, Code = "EUR", Name = "Euro" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("EUR", result.Code);
    }

    [Fact]
    public async Task CreateAsync_Cree_Et_Retourne_La_Devise()
    {
        // Le repository "retourne" l'entité avec un Id après ajout.
        var created = new Currency { Id = 1, Code = "GBP", Name = "Livre" };
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Currency>()))
            .ReturnsAsync(created);

        var dto = new CreateCurrencyDto { Code = "GBP", Name = "Livre" };
        var result = await _service.CreateAsync(dto);

        Assert.Equal(1, result.Id);
        Assert.Equal("GBP", result.Code);
        Assert.Equal("Livre", result.Name);
        // Vérifier que AddAsync a bien été appelé une fois.
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Currency>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Retourne_Null_Si_Devise_Inexistante()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Currency?)null);

        var dto = new CreateCurrencyDto { Code = "X", Name = "X" };
        var result = await _service.UpdateAsync(999, dto);

        Assert.Null(result);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Currency>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Ne_Plante_Pas_Si_Devise_Inexistante()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Currency?)null);

        // On s'attend à ce qu'aucune exception ne soit levée.
        await _service.DeleteAsync(999);

        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Currency>()), Times.Never);
    }
}
