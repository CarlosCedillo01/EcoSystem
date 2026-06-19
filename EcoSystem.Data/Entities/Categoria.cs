namespace EcoSystem.Data.Entities;

/// <summary>
/// Representa una categoría de productos en la tienda.
/// </summary>
public class Categoria
{
    public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    // Navegación
    public ICollection<Producto> Productos { get; set; } = [];
}
