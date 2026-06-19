namespace EcoSystem.Data.Entities;

/// <summary>
/// Representa una línea de detalle dentro de una orden (producto, cantidad y precio unitario).
/// </summary>
public class DetalleOrden
{
    public int IdDetalle { get; set; }
    public int? IdOrden { get; set; }
    public int? IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    // Navegación
    public Orden? Orden { get; set; }
    public Producto? Producto { get; set; }
}
