namespace EcoSystem.Business.Dtos.Clientes;

public sealed record ClienteUpdateDto(string Nombre, string? Email, string? Ciudad, DateOnly? FechaRegistro);
