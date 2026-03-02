using Conversion.API.DTOs.CurrencyRate;
using Conversion.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Conversion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyRatesController : ControllerBase
{
    private readonly ICurrencyRateService _currencyRateService;

    public CurrencyRatesController(ICurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CurrencyRateDto>>> GetAll()
    {
        var list = await _currencyRateService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CurrencyRateDto>> GetById(int id)
    {
        var rate = await _currencyRateService.GetByIdAsync(id);
        if (rate is null)
            return NotFound();
        return Ok(rate);
    }

    [HttpGet("from/{currencyFromId:int}/to/{currencyToId:int}")]
    public async Task<ActionResult<CurrencyRateDto>> GetRate(int currencyFromId, int currencyToId)
    {
        var rate = await _currencyRateService.GetRateAsync(currencyFromId, currencyToId);
        if (rate is null)
            return NotFound();
        return Ok(rate);
    }

    [HttpPost]
    public async Task<ActionResult<CurrencyRateDto>> Create([FromBody] CreateCurrencyRateDto dto)
    {
        var rate = await _currencyRateService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = rate.Id }, rate);
    }
}
