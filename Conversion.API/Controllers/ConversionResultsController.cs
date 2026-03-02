using Conversion.API.DTOs.Conversion;
using Conversion.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Conversion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversionResultsController : ControllerBase
{
    private readonly IConversionResultService _conversionResultService;

    public ConversionResultsController(IConversionResultService conversionResultService)
    {
        _conversionResultService = conversionResultService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ConversionResultDto>>> GetAll()
    {
        var list = await _conversionResultService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConversionResultDto>> GetById(int id)
    {
        var result = await _conversionResultService.GetByIdAsync(id);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ConversionResultDto>> Create([FromBody] CreateConversionResultDto dto)
    {
        try
        {
            var result = await _conversionResultService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
