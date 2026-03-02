using Conversion.API.DTOs.Conversion;

namespace Conversion.API.Services.Interfaces;

public interface IConversionResultService
{
    Task<List<ConversionResultDto>> GetAllAsync();
    Task<ConversionResultDto?> GetByIdAsync(int id);
    Task<ConversionResultDto> CreateAsync(CreateConversionResultDto dto);
}
