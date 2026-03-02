using Conversion.API.Models;

namespace Conversion.API.Repositories.Interfaces;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetAllAsync();
    Task<Currency?> GetByIdAsync(int id);
    Task<Currency?> GetByCodeAsync(string code);
    Task<Currency> AddAsync(Currency currency);
    Task<Currency> UpdateAsync(Currency currency);
    Task DeleteAsync(Currency currency);
}
