namespace EcoSystem.Business.Dtos.Productos;

public sealed record ProductoReadDto(int IdProducto, string Nombre, decimal Precio, int Stock, int? IdCategoria, string? CategoriaNombre);
