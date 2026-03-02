namespace Conversion.API.Models;

public class ConversionResult
{
    public int Id { get; set; }
    public int CurrencyFromId { get; set; }
    public int CurrencyToId { get; set; }
    public decimal Rate { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }

    public Currency CurrencyFrom { get; set; } = null!;
    public Currency CurrencyTo { get; set; } = null!;
}
