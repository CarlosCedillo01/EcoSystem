using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Productos;
using EcoSystem.Business.Interfaces;
using EcoSystem.Data.Context;
using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoSystem.Business.Services;

public sealed class ProductoService(EcoSystemDbContext context) : IProductoService
{
    public async Task<IReadOnlyList<ProductoReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .OrderBy(p => p.Nombre)
            .Select(p => new ProductoReadDto(p.IdProducto, p.Nombre, p.Precio, p.Stock, p.IdCategoria, p.Categoria != null ? p.Categoria.NombreCategoria : null))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceResult<ProductoReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var producto = await context.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.IdProducto == id, cancellationToken);

        return producto is null
            ? ServiceResult<ProductoReadDto>.Failure(404, "El producto no existe.")
            : ServiceResult<ProductoReadDto>.Success(Map(producto), "Producto encontrado.");
    }

    public async Task<ServiceResult<ProductoReadDto>> CreateAsync(ProductoCreateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateAsync(dto, cancellationToken);
        if (errors.Count > 0)
        {
            return ServiceResult<ProductoReadDto>.Failure(400, "No se pudo crear el producto.", errors.ToArray());
        }

        var producto = new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock,
            IdCategoria = dto.IdCategoria
        };

        context.Productos.Add(producto);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<ProductoReadDto>.Created(
            new ProductoReadDto(producto.IdProducto, producto.Nombre, producto.Precio, producto.Stock, producto.IdCategoria, await GetCategoriaNombreAsync(producto.IdCategoria, cancellationToken)),
            "Producto creado correctamente.");
    }

    public async Task<ServiceResult<ProductoReadDto>> UpdateAsync(int id, ProductoUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateAsync(dto, cancellationToken);
        if (errors.Count > 0)
        {
            return ServiceResult<ProductoReadDto>.Failure(400, "No se pudo actualizar el producto.", errors.ToArray());
        }

        var producto = await context.Productos.FirstOrDefaultAsync(p => p.IdProducto == id, cancellationToken);
        if (producto is null)
        {
            return ServiceResult<ProductoReadDto>.Failure(404, "El producto no existe.");
        }

        producto.Nombre = dto.Nombre.Trim();
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        producto.IdCategoria = dto.IdCategoria;

        await context.SaveChangesAsync(cancellationToken);

        var categoriaNombre = await GetCategoriaNombreAsync(producto.IdCategoria, cancellationToken);
        return ServiceResult<ProductoReadDto>.Success(new ProductoReadDto(producto.IdProducto, producto.Nombre, producto.Precio, producto.Stock, producto.IdCategoria, categoriaNombre), "Producto actualizado correctamente.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var producto = await context.Productos.FirstOrDefaultAsync(p => p.IdProducto == id, cancellationToken);
        if (producto is null)
        {
            return ServiceResult<bool>.Failure(404, "El producto no existe.");
        }

        context.Productos.Remove(producto);
        await context.SaveChangesAsync(cancellationToken);
        return ServiceResult<bool>.NoContent("Producto eliminado correctamente.");
    }

    private static ProductoReadDto Map(Producto producto)
        => new(producto.IdProducto, producto.Nombre, producto.Precio, producto.Stock, producto.IdCategoria, producto.Categoria?.NombreCategoria);

    private async Task<string?> GetCategoriaNombreAsync(int? idCategoria, CancellationToken cancellationToken)
    {
        if (idCategoria is null)
        {
            return null;
        }

        return await context.Categorias
            .AsNoTracking()
            .Where(c => c.IdCategoria == idCategoria)
            .Select(c => c.NombreCategoria)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<List<string>> ValidateAsync(ProductoCreateDto dto, CancellationToken cancellationToken)
        => await ValidateAsync(dto.Nombre, dto.Precio, dto.Stock, dto.IdCategoria, cancellationToken);

    private async Task<List<string>> ValidateAsync(ProductoUpdateDto dto, CancellationToken cancellationToken)
        => await ValidateAsync(dto.Nombre, dto.Precio, dto.Stock, dto.IdCategoria, cancellationToken);

    private async Task<List<string>> ValidateAsync(string nombre, decimal precio, int stock, int? idCategoria, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            errors.Add("El nombre del producto es obligatorio.");
        }
        else if (nombre.Trim().Length > 100)
        {
            errors.Add("El nombre del producto no puede superar 100 caracteres.");
        }

        if (precio <= 0)
        {
            errors.Add("El precio debe ser mayor que cero.");
        }

        if (stock < 0)
        {
            errors.Add("El stock no puede ser negativo.");
        }

        if (idCategoria is not null && !await context.Categorias.AnyAsync(c => c.IdCategoria == idCategoria, cancellationToken))
        {
            errors.Add("La categoría indicada no existe.");
        }

        return errors;
    }
}
