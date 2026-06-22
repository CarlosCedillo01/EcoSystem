namespace EcoSystem.Business.Dtos.Clientes;

public sealed record ClienteReadDto(int IdCliente, string Nombre, string? Email, string? Ciudad, DateOnly? FechaRegistro);
