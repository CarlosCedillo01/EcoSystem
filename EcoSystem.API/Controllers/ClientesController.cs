using EcoSystem.Business.Dtos.Clientes;
using EcoSystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcoSystem.API.Controllers;

[ApiController]
[Route("api/clientes")]
public sealed class ClientesController(IClienteService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClienteReadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var clientes = await service.GetAllAsync(cancellationToken);
        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<ClienteReadDto>.From(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, ApiResponse<ClienteReadDto>.From(result));
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdCliente }, ApiResponse<ClienteReadDto>.From(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<ClienteReadDto>.From(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, ApiResponse<bool>.From(result));
    }
}
