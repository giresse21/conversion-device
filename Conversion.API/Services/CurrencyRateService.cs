using Conversion.API.DTOs.CurrencyRate;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Conversion.API.Services.Interfaces;

namespace Conversion.API.Services;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly ICurrencyRateRepository _currencyRateRepository;

    public CurrencyRateService(ICurrencyRateRepository currencyRateRepository)
    {
        _currencyRateRepository = currencyRateRepository;
    }

    public async Task<List<CurrencyRateDto>> GetAllAsync()
    {
        var list = await _currencyRateRepository.GetAllAsync();
        return list.Select(MapToDto).ToList();
    }

    public async Task<CurrencyRateDto?> GetByIdAsync(int id)
    {
        var entity = await _currencyRateRepository.GetByIdAsync(id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<CurrencyRateDto?> GetRateAsync(int currencyFromId, int currencyToId)
    {
        var entity = await _currencyRateRepository.GetRateAsync(currencyFromId, currencyToId);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<CurrencyRateDto> CreateAsync(CreateCurrencyRateDto dto)
    {
        var entity = new CurrencyRate
        {
            CurrencyFromId = dto.CurrencyFromId,
            CurrencyToId = dto.CurrencyToId,
            Rate = dto.Rate
        };
        entity = await _currencyRateRepository.AddAsync(entity);
        var withNav = await _currencyRateRepository.GetByIdAsync(entity.Id);
        return MapToDto(withNav!);
    }

    private static CurrencyRateDto MapToDto(CurrencyRate entity)
    {
        return new CurrencyRateDto
        {
            Id = entity.Id,
            CurrencyFromCode = entity.CurrencyFrom.Code,
            CurrencyToCode = entity.CurrencyTo.Code,
            Rate = entity.Rate
        };
    }
}
