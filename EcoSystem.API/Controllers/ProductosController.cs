using EcoSystem.Business.Dtos.Productos;
using EcoSystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcoSystem.API.Controllers;

[ApiController]
[Route("api/productos")]
public sealed class ProductosController(IProductoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductoReadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var productos = await service.GetAllAsync(cancellationToken);
        return Ok(productos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<ProductoReadDto>.From(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, ApiResponse<ProductoReadDto>.From(result));
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdProducto }, ApiResponse<ProductoReadDto>.From(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<ProductoReadDto>.From(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, ApiResponse<bool>.From(result));
    }
}
