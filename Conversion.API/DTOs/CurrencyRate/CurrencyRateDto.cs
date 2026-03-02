namespace Conversion.API.DTOs.CurrencyRate;

public class CurrencyRateDto
{
    public int Id { get; set; }
    public string CurrencyFromCode { get; set; } = string.Empty;
    public string CurrencyToCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
}
