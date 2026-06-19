using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoSystem.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.IdCliente);

        builder.Property(c => c.IdCliente)
            .HasColumnName("id_cliente")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasMaxLength(100);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.Ciudad)
            .HasColumnName("ciudad")
            .HasMaxLength(60);

        builder.Property(c => c.FechaRegistro)
            .HasColumnName("fecha_registro");

        // Datos semilla
        builder.HasData(
            new Cliente { IdCliente = 1, Nombre = "Ana García", Email = "ana@email.com", Ciudad = "CDMX", FechaRegistro = new DateOnly(2023, 1, 15) },
            new Cliente { IdCliente = 2, Nombre = "Luis Martínez", Email = "luis@email.com", Ciudad = "Guadalajara", FechaRegistro = new DateOnly(2023, 3, 22) },
            new Cliente { IdCliente = 3, Nombre = "María López", Email = "maria@email.com", Ciudad = "Monterrey", FechaRegistro = new DateOnly(2023, 6, 10) },
            new Cliente { IdCliente = 4, Nombre = "Carlos Ruiz", Email = "carlos@email.com", Ciudad = "CDMX", FechaRegistro = new DateOnly(2024, 1, 5) },
            new Cliente { IdCliente = 5, Nombre = "Sofía Torres", Email = null, Ciudad = "Puebla", FechaRegistro = new DateOnly(2024, 2, 20) }
        );
    }
}
