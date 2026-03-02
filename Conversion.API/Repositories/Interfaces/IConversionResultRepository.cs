using Conversion.API.Models;

namespace Conversion.API.Repositories.Interfaces;

public interface IConversionResultRepository
{
    Task<List<ConversionResult>> GetAllAsync();
    Task<ConversionResult?> GetByIdAsync(int id);
    Task<ConversionResult> AddAsync(ConversionResult conversionResult);
}
