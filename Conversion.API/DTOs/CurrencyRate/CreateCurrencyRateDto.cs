namespace Conversion.API.DTOs.CurrencyRate;

public class CreateCurrencyRateDto
{
    public int CurrencyFromId { get; set; }
    public int CurrencyToId { get; set; }
    public decimal Rate { get; set; }
}
