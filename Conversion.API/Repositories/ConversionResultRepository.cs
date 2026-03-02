using Conversion.API.Data;
using Conversion.API.Models;
using Conversion.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Conversion.API.Repositories;

public class ConversionResultRepository : IConversionResultRepository
{
    private readonly AppDbContext _context;

    public ConversionResultRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ConversionResult>> GetAllAsync()
    {
        return await _context.ConversionResults
            .Include(c => c.CurrencyFrom)
            .Include(c => c.CurrencyTo)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<ConversionResult?> GetByIdAsync(int id)
    {
        return await _context.ConversionResults
            .Include(c => c.CurrencyFrom)
            .Include(c => c.CurrencyTo)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ConversionResult> AddAsync(ConversionResult conversionResult)
    {
        _context.ConversionResults.Add(conversionResult);
        await _context.SaveChangesAsync();
        return conversionResult;
    }
}
