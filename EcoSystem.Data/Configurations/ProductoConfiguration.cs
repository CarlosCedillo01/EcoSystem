using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoSystem.Data.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("productos");

        builder.HasKey(p => p.IdProducto);

        builder.Property(p => p.IdProducto)
            .HasColumnName("id_producto")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Precio)
            .HasColumnName("precio")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.Stock)
            .HasColumnName("stock")
            .HasDefaultValue(0);

        builder.Property(p => p.IdCategoria)
            .HasColumnName("id_categoria");

        // Relación: Producto -> Categoria
        builder.HasOne(p => p.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(p => p.IdCategoria)
            .OnDelete(DeleteBehavior.SetNull);

        // Datos semilla
        builder.HasData(
            new Producto { IdProducto = 1, Nombre = "Laptop Pro 15", Precio = 18500.00m, Stock = 10, IdCategoria = 1 },
            new Producto { IdProducto = 2, Nombre = "Teclado Mecánico", Precio = 950.00m, Stock = 25, IdCategoria = 1 },
            new Producto { IdProducto = 3, Nombre = "Playera Casual", Precio = 280.00m, Stock = 100, IdCategoria = 2 },
            new Producto { IdProducto = 4, Nombre = "Cafetera Espresso", Precio = 1200.00m, Stock = 15, IdCategoria = 3 },
            new Producto { IdProducto = 5, Nombre = "Cámara Mirrorless", Precio = 22000.00m, Stock = 5, IdCategoria = 1 },
            new Producto { IdProducto = 6, Nombre = "SQL para Todos", Precio = 350.00m, Stock = 40, IdCategoria = 4 },
            new Producto { IdProducto = 7, Nombre = "Auriculares BT", Precio = 1500.00m, Stock = 0, IdCategoria = 1 }
        );
    }
}
