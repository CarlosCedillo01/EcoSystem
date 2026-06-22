using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Categorias;
using EcoSystem.Business.Interfaces;
using EcoSystem.Data.Context;
using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoSystem.Business.Services;

public sealed class CategoriaService(EcoSystemDbContext context) : ICategoriaService
{
    public async Task<IReadOnlyList<CategoriaReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.NombreCategoria)
            .Select(c => new CategoriaReadDto(c.IdCategoria, c.NombreCategoria, c.Descripcion))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceResult<CategoriaReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoria = await context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCategoria == id, cancellationToken);

        return categoria is null
            ? ServiceResult<CategoriaReadDto>.Failure(404, "La categoría no existe.")
            : ServiceResult<CategoriaReadDto>.Success(Map(categoria), "Categoría encontrada.");
    }

    public async Task<ServiceResult<CategoriaReadDto>> CreateAsync(CategoriaCreateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = Validate(dto);
        if (errors.Count > 0)
        {
            return ServiceResult<CategoriaReadDto>.Failure(400, "No se pudo crear la categoría.", errors.ToArray());
        }

        var categoria = new Categoria
        {
            NombreCategoria = dto.NombreCategoria.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim()
        };

        context.Categorias.Add(categoria);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<CategoriaReadDto>.Created(Map(categoria), "Categoría creada correctamente.");
    }

    public async Task<ServiceResult<CategoriaReadDto>> UpdateAsync(int id, CategoriaUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = Validate(dto);
        if (errors.Count > 0)
        {
            return ServiceResult<CategoriaReadDto>.Failure(400, "No se pudo actualizar la categoría.", errors.ToArray());
        }

        var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == id, cancellationToken);
        if (categoria is null)
        {
            return ServiceResult<CategoriaReadDto>.Failure(404, "La categoría no existe.");
        }

        categoria.NombreCategoria = dto.NombreCategoria.Trim();
        categoria.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();

        await context.SaveChangesAsync(cancellationToken);
        return ServiceResult<CategoriaReadDto>.Success(Map(categoria), "Categoría actualizada correctamente.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == id, cancellationToken);
        if (categoria is null)
        {
            return ServiceResult<bool>.Failure(404, "La categoría no existe.");
        }

        context.Categorias.Remove(categoria);
        await context.SaveChangesAsync(cancellationToken);
        return ServiceResult<bool>.NoContent("Categoría eliminada correctamente.");
    }

    private static CategoriaReadDto Map(Categoria categoria)
        => new(categoria.IdCategoria, categoria.NombreCategoria, categoria.Descripcion);

    private static List<string> Validate(CategoriaCreateDto dto)
        => Validate(dto.NombreCategoria, dto.Descripcion);

    private static List<string> Validate(CategoriaUpdateDto dto)
        => Validate(dto.NombreCategoria, dto.Descripcion);

    private static List<string> Validate(string nombreCategoria, string? descripcion)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(nombreCategoria))
        {
            errors.Add("El nombre de la categoría es obligatorio.");
        }
        else if (nombreCategoria.Trim().Length > 50)
        {
            errors.Add("El nombre de la categoría no puede superar 50 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Trim().Length > 250)
        {
            errors.Add("La descripción no puede superar 250 caracteres.");
        }

        return errors;
    }
}
