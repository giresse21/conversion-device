using Conversion.API.Data;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Conversion.API.Repositories;

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly AppDbContext _context;

    public CurrencyRateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CurrencyRate>> GetAllAsync()
    {
        return await _context.CurrencyRates
            .Include(cr => cr.CurrencyFrom)
            .Include(cr => cr.CurrencyTo)
            .ToListAsync();
    }

    public async Task<CurrencyRate?> GetByIdAsync(int id)
    {
        return await _context.CurrencyRates
            .Include(cr => cr.CurrencyFrom)
            .Include(cr => cr.CurrencyTo)
            .FirstOrDefaultAsync(cr => cr.Id == id);
    }

    public async Task<CurrencyRate?> GetRateAsync(int currencyFromId, int currencyToId)
    {
        return await _context.CurrencyRates
            .Include(cr => cr.CurrencyFrom)
            .Include(cr => cr.CurrencyTo)
            .FirstOrDefaultAsync(
                cr => cr.CurrencyFromId == currencyFromId && cr.CurrencyToId == currencyToId);
    }

    public async Task<CurrencyRate> AddAsync(CurrencyRate currencyRate)
    {
        _context.CurrencyRates.Add(currencyRate);
        await _context.SaveChangesAsync();
        return currencyRate;
    }
}
