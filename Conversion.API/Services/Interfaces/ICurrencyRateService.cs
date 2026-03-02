using Conversion.API.DTOs.CurrencyRate;

namespace Conversion.API.Services.Interfaces;

public interface ICurrencyRateService
{
    Task<List<CurrencyRateDto>> GetAllAsync();
    Task<CurrencyRateDto?> GetByIdAsync(int id);
    Task<CurrencyRateDto?> GetRateAsync(int currencyFromId, int currencyToId);
    Task<CurrencyRateDto> CreateAsync(CreateCurrencyRateDto dto);
}
