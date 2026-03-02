using Conversion.API.Models;

namespace Conversion.API.Repositories.Interfaces;

public interface ICurrencyRateRepository
{
    Task<List<CurrencyRate>> GetAllAsync();
    Task<CurrencyRate?> GetByIdAsync(int id);
    Task<CurrencyRate?> GetRateAsync(int currencyFromId, int currencyToId);
    Task<CurrencyRate> AddAsync(CurrencyRate currencyRate);
}
