namespace EcoSystem.Data.Entities;

/// <summary>
/// Representa un cliente registrado en la tienda.
/// </summary>
public class Cliente
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Ciudad { get; set; }
    public DateOnly? FechaRegistro { get; set; }

    // Navegación
    public ICollection<Orden> Ordenes { get; set; } = [];
}
