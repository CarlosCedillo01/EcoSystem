namespace EcoSystem.Business.Dtos.Productos;

public sealed record ProductoUpdateDto(string Nombre, decimal Precio, int Stock, int? IdCategoria);
