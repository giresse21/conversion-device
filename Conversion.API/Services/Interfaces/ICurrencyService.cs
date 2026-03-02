using Conversion.API.DTOs.Currency;

namespace Conversion.API.Services.Interfaces;

public interface ICurrencyService
{
    Task<List<CurrencyDto>> GetAllAsync();
    Task<CurrencyDto?> GetByIdAsync(int id);
    Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto);
    Task<CurrencyDto?> UpdateAsync(int id, CreateCurrencyDto dto);
    Task DeleteAsync(int id);
}
