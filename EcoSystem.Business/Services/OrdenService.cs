using EcoSystem.Business.Common;
using EcoSystem.Business.Dtos.Ordenes;
using EcoSystem.Business.Interfaces;
using EcoSystem.Data.Context;
using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoSystem.Business.Services;

public sealed class OrdenService(EcoSystemDbContext context) : IOrdenService
{
    public async Task<IReadOnlyList<OrdenReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var ordenes = await LoadOrdenesAsync()
            .OrderByDescending(o => o.FechaOrden)
            .ToListAsync(cancellationToken);

        return ordenes.Select(Map).ToList();
    }

    public async Task<ServiceResult<OrdenReadDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var orden = await LoadOrdenesAsync()
            .FirstOrDefaultAsync(o => o.IdOrden == id, cancellationToken);

        return orden is null
            ? ServiceResult<OrdenReadDto>.Failure(404, "La orden no existe.")
            : ServiceResult<OrdenReadDto>.Success(Map(orden), "Orden encontrada.");
    }

    public async Task<ServiceResult<OrdenReadDto>> CreateAsync(OrdenCreateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateAsync(dto.IdCliente, dto.Detalles, cancellationToken);
        if (errors.Count > 0)
        {
            return ServiceResult<OrdenReadDto>.Failure(400, "No se pudo crear la orden.", errors.ToArray());
        }

        var orden = new Orden
        {
            IdCliente = dto.IdCliente,
            FechaOrden = dto.FechaOrden ?? DateTime.UtcNow,
            Estado = dto.Estado
        };

        context.Ordenes.Add(orden);
        await context.SaveChangesAsync(cancellationToken);

        var detalles = dto.Detalles.Select(detalle => new DetalleOrden
        {
            IdOrden = orden.IdOrden,
            IdProducto = detalle.IdProducto,
            Cantidad = detalle.Cantidad,
            PrecioUnitario = detalle.PrecioUnitario
        }).ToList();

        context.DetalleOrdenes.AddRange(detalles);
        await context.SaveChangesAsync(cancellationToken);

        var created = await LoadOrdenesAsync()
            .FirstAsync(o => o.IdOrden == orden.IdOrden, cancellationToken);

        return ServiceResult<OrdenReadDto>.Created(Map(created), "Orden creada correctamente.");
    }

    public async Task<ServiceResult<OrdenReadDto>> UpdateAsync(int id, OrdenUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateAsync(dto.IdCliente, dto.Detalles, cancellationToken);
        if (errors.Count > 0)
        {
            return ServiceResult<OrdenReadDto>.Failure(400, "No se pudo actualizar la orden.", errors.ToArray());
        }

        var orden = await context.Ordenes
            .Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.IdOrden == id, cancellationToken);

        if (orden is null)
        {
            return ServiceResult<OrdenReadDto>.Failure(404, "La orden no existe.");
        }

        orden.IdCliente = dto.IdCliente;
        orden.FechaOrden = dto.FechaOrden ?? orden.FechaOrden;
        orden.Estado = dto.Estado;

        context.DetalleOrdenes.RemoveRange(orden.Detalles);
        await context.SaveChangesAsync(cancellationToken);

        var detalles = dto.Detalles.Select(detalle => new DetalleOrden
        {
            IdOrden = orden.IdOrden,
            IdProducto = detalle.IdProducto,
            Cantidad = detalle.Cantidad,
            PrecioUnitario = detalle.PrecioUnitario
        }).ToList();

        context.DetalleOrdenes.AddRange(detalles);
        await context.SaveChangesAsync(cancellationToken);

        var updated = await LoadOrdenesAsync()
            .FirstAsync(o => o.IdOrden == orden.IdOrden, cancellationToken);

        return ServiceResult<OrdenReadDto>.Success(Map(updated), "Orden actualizada correctamente.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var orden = await context.Ordenes
            .Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.IdOrden == id, cancellationToken);

        if (orden is null)
        {
            return ServiceResult<bool>.Failure(404, "La orden no existe.");
        }

        context.Ordenes.Remove(orden);
        await context.SaveChangesAsync(cancellationToken);
        return ServiceResult<bool>.NoContent("Orden eliminada correctamente.");
    }

    private IQueryable<Orden> LoadOrdenesAsync()
    {
        return context.Ordenes
            .AsNoTracking()
            .Include(o => o.Cliente)
            .Include(o => o.Detalles)
            .ThenInclude(d => d.Producto);
    }

    private static OrdenReadDto Map(Orden orden)
    {
        var detalles = orden.Detalles
            .OrderBy(d => d.IdDetalle)
            .Select(d => new DetalleOrdenReadDto(
                d.IdDetalle,
                d.IdProducto ?? 0,
                d.Producto?.Nombre,
                d.Cantidad,
                d.PrecioUnitario,
                d.Cantidad * d.PrecioUnitario))
            .ToList();

        var total = detalles.Sum(d => d.Subtotal);

        return new OrdenReadDto(
            orden.IdOrden,
            orden.IdCliente,
            orden.Cliente?.Nombre,
            orden.FechaOrden,
            orden.Estado,
            detalles,
            total);
    }

    private async Task<List<string>> ValidateAsync(int? idCliente, IReadOnlyCollection<DetalleOrdenCreateDto> detalles, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (idCliente is not null && !await context.Clientes.AnyAsync(c => c.IdCliente == idCliente, cancellationToken))
        {
            errors.Add("El cliente indicado no existe.");
        }

        if (detalles is null || detalles.Count == 0)
        {
            errors.Add("La orden debe incluir al menos un detalle.");
            return errors;
        }

        var productosRepetidos = new HashSet<int>();
        foreach (var detalle in detalles)
        {
            if (!productosRepetidos.Add(detalle.IdProducto))
            {
                errors.Add($"El producto {detalle.IdProducto} está repetido en los detalles.");
            }

            if (detalle.Cantidad <= 0)
            {
                errors.Add($"La cantidad del producto {detalle.IdProducto} debe ser mayor que cero.");
            }

            if (detalle.PrecioUnitario <= 0)
            {
                errors.Add($"El precio unitario del producto {detalle.IdProducto} debe ser mayor que cero.");
            }
        }

        var idsProductos = detalles.Select(d => d.IdProducto).Distinct().ToArray();
        var existentes = await context.Productos
            .AsNoTracking()
            .Where(p => idsProductos.Contains(p.IdProducto))
            .Select(p => p.IdProducto)
            .ToListAsync(cancellationToken);

        foreach (var idProducto in idsProductos.Except(existentes))
        {
            errors.Add($"El producto {idProducto} no existe.");
        }

        return errors;
    }
}
