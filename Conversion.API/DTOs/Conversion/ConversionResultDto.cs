namespace Conversion.API.DTOs.Conversion;

public class ConversionResultDto
{
    public int Id { get; set; }
    public string CurrencyFromCode { get; set; } = string.Empty;
    public string CurrencyToCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
}
