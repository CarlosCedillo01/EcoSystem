namespace EcoSystem.Business.Dtos.Clientes;

public sealed record ClienteCreateDto(string Nombre, string? Email, string? Ciudad, DateOnly? FechaRegistro);
