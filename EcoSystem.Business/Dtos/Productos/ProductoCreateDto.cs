namespace EcoSystem.Business.Dtos.Productos;

public sealed record ProductoCreateDto(string Nombre, decimal Precio, int Stock, int? IdCategoria);
