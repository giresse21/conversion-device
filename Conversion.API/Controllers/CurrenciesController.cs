using Conversion.API.DTOs.Currency;
using Conversion.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Conversion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrenciesController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrenciesController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CurrencyDto>>> GetAll()
    {
        var list = await _currencyService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CurrencyDto>> GetById(int id)
    {
        var currency = await _currencyService.GetByIdAsync(id);
        if (currency is null)
            return NotFound();
        return Ok(currency);
    }

    [HttpPost]
    public async Task<ActionResult<CurrencyDto>> Create([FromBody] CreateCurrencyDto dto)
    {
        var currency = await _currencyService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = currency.Id }, currency);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CurrencyDto>> Update(int id, [FromBody] CreateCurrencyDto dto)
    {
        var currency = await _currencyService.UpdateAsync(id, dto);
        if (currency is null)
            return NotFound();
        return Ok(currency);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _currencyService.DeleteAsync(id);
        return NoContent();
    }
}
