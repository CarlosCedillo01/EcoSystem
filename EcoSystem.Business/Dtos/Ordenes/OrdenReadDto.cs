using EcoSystem.Data.Enums;

namespace EcoSystem.Business.Dtos.Ordenes;

public sealed record OrdenReadDto(
    int IdOrden,
    int? IdCliente,
    string? ClienteNombre,
    DateTime FechaOrden,
    EstadoOrden Estado,
    IReadOnlyCollection<DetalleOrdenReadDto> Detalles,
    decimal Total);
