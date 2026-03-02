using Conversion.API.DTOs.Currency;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Conversion.API.Services.Interfaces;

namespace Conversion.API.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<List<CurrencyDto>> GetAllAsync()
    {
        var list = await _currencyRepository.GetAllAsync();
        return list.Select(MapToDto).ToList();
    }

    public async Task<CurrencyDto?> GetByIdAsync(int id)
    {
        var entity = await _currencyRepository.GetByIdAsync(id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto)
    {
        var entity = new Currency
        {
            Code = dto.Code,
            Name = dto.Name
        };
        entity = await _currencyRepository.AddAsync(entity);
        return MapToDto(entity);
    }

    public async Task<CurrencyDto?> UpdateAsync(int id, CreateCurrencyDto dto)
    {
        var entity = await _currencyRepository.GetByIdAsync(id);
        if (entity is null)
            return null;

        entity.Code = dto.Code;
        entity.Name = dto.Name;
        entity = await _currencyRepository.UpdateAsync(entity);
        return MapToDto(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _currencyRepository.GetByIdAsync(id);
        if (entity is not null)
            await _currencyRepository.DeleteAsync(entity);
    }

    private static CurrencyDto MapToDto(Currency entity)
    {
        return new CurrencyDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name
        };
    }
}
