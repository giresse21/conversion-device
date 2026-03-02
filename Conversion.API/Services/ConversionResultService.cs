using Conversion.API.DTOs.Conversion;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Conversion.API.Services.Interfaces;

namespace Conversion.API.Services;

public class ConversionResultService : IConversionResultService
{
    private readonly IConversionResultRepository _conversionResultRepository;
    private readonly ICurrencyRateRepository _currencyRateRepository;

    public ConversionResultService(
        IConversionResultRepository conversionResultRepository,
        ICurrencyRateRepository currencyRateRepository)
    {
        _conversionResultRepository = conversionResultRepository;
        _currencyRateRepository = currencyRateRepository;
    }

    public async Task<List<ConversionResultDto>> GetAllAsync()
    {
        var list = await _conversionResultRepository.GetAllAsync();
        return list.Select(MapToDto).ToList();
    }

    public async Task<ConversionResultDto?> GetByIdAsync(int id)
    {
        var entity = await _conversionResultRepository.GetByIdAsync(id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<ConversionResultDto> CreateAsync(CreateConversionResultDto dto)
    {
        var rate = await _currencyRateRepository.GetRateAsync(dto.CurrencyFromId, dto.CurrencyToId);
        if (rate is null)
            throw new InvalidOperationException($"Aucun taux trouvé pour les devises {dto.CurrencyFromId} -> {dto.CurrencyToId}.");

        var entity = new ConversionResult
        {
            CurrencyFromId = dto.CurrencyFromId,
            CurrencyToId = dto.CurrencyToId,
            Rate = rate.Rate,
            Value = dto.Value,
            CreatedAt = DateTime.UtcNow
        };
        entity = await _conversionResultRepository.AddAsync(entity);
        return MapToDto(entity);
    }

    private static ConversionResultDto MapToDto(ConversionResult entity)
    {
        return new ConversionResultDto
        {
            Id = entity.Id,
            CurrencyFromCode = entity.CurrencyFrom.Code,
            CurrencyToCode = entity.CurrencyTo.Code,
            Rate = entity.Rate,
            Value = entity.Value,
            CreatedAt = entity.CreatedAt
        };
    }
}
