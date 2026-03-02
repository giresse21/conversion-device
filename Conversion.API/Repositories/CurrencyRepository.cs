using Conversion.API.Data;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Conversion.API.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly AppDbContext _context;

    public CurrencyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Currency>> GetAllAsync()
    {
        return await _context.Currencies
            .OrderBy(c => c.Code)
            .ToListAsync();
    }

    public async Task<Currency?> GetByIdAsync(int id)
    {
        return await _context.Currencies
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Currency?> GetByCodeAsync(string code)
    {
        return await _context.Currencies
            .FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task<Currency> AddAsync(Currency currency)
    {
        _context.Currencies.Add(currency);
        await _context.SaveChangesAsync();
        return currency;
    }

    public async Task<Currency> UpdateAsync(Currency currency)
    {
        _context.Currencies.Update(currency);
        await _context.SaveChangesAsync();
        return currency;
    }

    public async Task DeleteAsync(Currency currency)
    {
        _context.Currencies.Remove(currency);
        await _context.SaveChangesAsync();
    }
}
