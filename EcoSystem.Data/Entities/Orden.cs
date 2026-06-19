using EcoSystem.Data.Enums;

namespace EcoSystem.Data.Entities;

/// <summary>
/// Representa una orden de compra realizada por un cliente.
/// </summary>
public class Orden
{
    public int IdOrden { get; set; }
    public int? IdCliente { get; set; }
    public DateTime FechaOrden { get; set; }
    public EstadoOrden Estado { get; set; }

    // Navegación
    public Cliente? Cliente { get; set; }
    public ICollection<DetalleOrden> Detalles { get; set; } = [];
}
