using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoSystem.Data.Configurations;

public class DetalleOrdenConfiguration : IEntityTypeConfiguration<DetalleOrden>
{
    public void Configure(EntityTypeBuilder<DetalleOrden> builder)
    {
        builder.ToTable("detalle_ordenes");

        builder.HasKey(d => d.IdDetalle);

        builder.Property(d => d.IdDetalle)
            .HasColumnName("id_detalle")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.IdOrden)
            .HasColumnName("id_orden");

        builder.Property(d => d.IdProducto)
            .HasColumnName("id_producto");

        builder.Property(d => d.Cantidad)
            .HasColumnName("cantidad")
            .IsRequired();

        builder.Property(d => d.PrecioUnitario)
            .HasColumnName("precio_unitario")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // Relación: DetalleOrden -> Orden
        builder.HasOne(d => d.Orden)
            .WithMany(o => o.Detalles)
            .HasForeignKey(d => d.IdOrden)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación: DetalleOrden -> Producto
        builder.HasOne(d => d.Producto)
            .WithMany(p => p.DetallesOrdenes)
            .HasForeignKey(d => d.IdProducto)
            .OnDelete(DeleteBehavior.SetNull);

        // Datos semilla
        builder.HasData(
            new DetalleOrden { IdDetalle = 1, IdOrden = 1, IdProducto = 1, Cantidad = 1, PrecioUnitario = 18500.00m },
            new DetalleOrden { IdDetalle = 2, IdOrden = 1, IdProducto = 2, Cantidad = 2, PrecioUnitario = 950.00m },
            new DetalleOrden { IdDetalle = 3, IdOrden = 2, IdProducto = 3, Cantidad = 3, PrecioUnitario = 280.00m },
            new DetalleOrden { IdDetalle = 4, IdOrden = 3, IdProducto = 4, Cantidad = 1, PrecioUnitario = 1200.00m },
            new DetalleOrden { IdDetalle = 5, IdOrden = 3, IdProducto = 6, Cantidad = 2, PrecioUnitario = 350.00m },
            new DetalleOrden { IdDetalle = 6, IdOrden = 4, IdProducto = 5, Cantidad = 1, PrecioUnitario = 22000.00m },
            new DetalleOrden { IdDetalle = 7, IdOrden = 5, IdProducto = 2, Cantidad = 1, PrecioUnitario = 950.00m }
        );
    }
}
