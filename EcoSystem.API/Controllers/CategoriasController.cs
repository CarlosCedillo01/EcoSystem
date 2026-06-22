using EcoSystem.Business.Dtos.Categorias;
using EcoSystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcoSystem.API.Controllers;

[ApiController]
[Route("api/categorias")]
public sealed class CategoriasController(ICategoriaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoriaReadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var categorias = await service.GetAllAsync(cancellationToken);
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<CategoriaReadDto>.From(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoriaCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(dto, cancellationToken);
        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, ApiResponse<CategoriaReadDto>.From(result));
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdCategoria }, ApiResponse<CategoriaReadDto>.From(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, dto, cancellationToken);
        return StatusCode(result.StatusCode, ApiResponse<CategoriaReadDto>.From(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, ApiResponse<bool>.From(result));
    }
}
