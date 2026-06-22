using EcoSystem.Data.Enums;

namespace EcoSystem.Business.Dtos.Ordenes;

public sealed record OrdenCreateDto(int? IdCliente, DateTime? FechaOrden, EstadoOrden Estado, IReadOnlyCollection<DetalleOrdenCreateDto> Detalles);
