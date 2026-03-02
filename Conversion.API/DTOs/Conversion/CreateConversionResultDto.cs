namespace Conversion.API.DTOs.Conversion;

public class CreateConversionResultDto
{
    public int CurrencyFromId { get; set; }
    public int CurrencyToId { get; set; }
    public decimal Value { get; set; }
}
