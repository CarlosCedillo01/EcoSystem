using EcoSystem.Data.Entities;
using EcoSystem.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoSystem.Data.Configurations;

public class OrdenConfiguration : IEntityTypeConfiguration<Orden>
{
    public void Configure(EntityTypeBuilder<Orden> builder)
    {
        builder.ToTable("ordenes");

        builder.HasKey(o => o.IdOrden);

        builder.Property(o => o.IdOrden)
            .HasColumnName("id_orden")
            .ValueGeneratedOnAdd();

        builder.Property(o => o.IdCliente)
            .HasColumnName("id_cliente");

        builder.Property(o => o.FechaOrden)
            .HasColumnName("fecha_orden")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Almacena el enum como string en PostgreSQL (equivalente al ENUM de MySQL)
        builder.Property(o => o.Estado)
            .HasColumnName("estado")
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relación: Orden -> Cliente
        builder.HasOne(o => o.Cliente)
            .WithMany(c => c.Ordenes)
            .HasForeignKey(o => o.IdCliente)
            .OnDelete(DeleteBehavior.SetNull);

        // Datos semilla
        builder.HasData(
            new Orden { IdOrden = 1, IdCliente = 1, FechaOrden = new DateTime(2024, 1, 10, 10, 30, 0, DateTimeKind.Utc), Estado = EstadoOrden.Entregado },
            new Orden { IdOrden = 2, IdCliente = 1, FechaOrden = new DateTime(2024, 2, 15, 14, 0, 0, DateTimeKind.Utc), Estado = EstadoOrden.Enviado },
            new Orden { IdOrden = 3, IdCliente = 2, FechaOrden = new DateTime(2024, 1, 20, 9, 0, 0, DateTimeKind.Utc), Estado = EstadoOrden.Entregado },
            new Orden { IdOrden = 4, IdCliente = 3, FechaOrden = new DateTime(2024, 3, 1, 16, 45, 0, DateTimeKind.Utc), Estado = EstadoOrden.Pendiente },
            new Orden { IdOrden = 5, IdCliente = 2, FechaOrden = new DateTime(2024, 3, 10, 11, 20, 0, DateTimeKind.Utc), Estado = EstadoOrden.Cancelado }
        );
    }
}
