namespace EcoSystem.Business.Dtos.Ordenes;

public sealed record DetalleOrdenCreateDto(int IdProducto, int Cantidad, decimal PrecioUnitario);
