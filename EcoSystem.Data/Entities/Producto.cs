namespace EcoSystem.Data.Entities;

/// <summary>
/// Representa un producto disponible en la tienda.
/// </summary>
public class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int? IdCategoria { get; set; }

    // Navegación
    public Categoria? Categoria { get; set; }
    public ICollection<DetalleOrden> DetallesOrdenes { get; set; } = [];
}
