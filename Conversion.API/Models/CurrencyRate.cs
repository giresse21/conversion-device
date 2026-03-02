namespace Conversion.API.Models;

public class CurrencyRate
{
    public int Id { get; set; }
    public int CurrencyFromId { get; set; }
    public int CurrencyToId { get; set; }
    public decimal Rate { get; set; }

    public Currency CurrencyFrom { get; set; } = null!;
    public Currency CurrencyTo { get; set; } = null!;
}
