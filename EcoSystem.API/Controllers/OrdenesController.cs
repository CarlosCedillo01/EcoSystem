using EcoSystem.Business.Dtos.Ordenes;
using EcoSystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcoSystem.API.Controllers;

[ApiController]
[Route("api/ordenes")]
public sealed class OrdenesController(IOrdenService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrdenReadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var ordenes = await service.GetAllAsync(cancellationToken);
        return Ok(ordenes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<OrdenReadDto>.From(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrdenCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, ApiResponse<OrdenReadDto>.From(result));
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdOrden }, ApiResponse<OrdenReadDto>.From(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrdenUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<OrdenReadDto>.From(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, ApiResponse<bool>.From(result));
    }
}
