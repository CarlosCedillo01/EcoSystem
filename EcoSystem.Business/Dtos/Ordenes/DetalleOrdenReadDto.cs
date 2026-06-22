namespace EcoSystem.Business.Dtos.Ordenes;

public sealed record DetalleOrdenReadDto(int IdDetalle, int IdProducto, string? ProductoNombre, int Cantidad, decimal PrecioUnitario, decimal Subtotal);
