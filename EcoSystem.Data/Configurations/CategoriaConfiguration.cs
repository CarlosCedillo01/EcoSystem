using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoSystem.Data.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("categorias");

        builder.HasKey(c => c.IdCategoria);

        builder.Property(c => c.IdCategoria)
            .HasColumnName("id_categoria")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.NombreCategoria)
            .HasColumnName("nombre_categoria")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Descripcion)
            .HasColumnName("descripcion")
            .HasColumnType("text");

        // Datos semilla
        builder.HasData(
            new Categoria { IdCategoria = 1, NombreCategoria = "Electrónica", Descripcion = "Gadgets y dispositivos" },
            new Categoria { IdCategoria = 2, NombreCategoria = "Ropa", Descripcion = "Prendas de vestir" },
            new Categoria { IdCategoria = 3, NombreCategoria = "Hogar", Descripcion = "Artículos para el hogar" },
            new Categoria { IdCategoria = 4, NombreCategoria = "Libros", Descripcion = "Literatura y técnicos" }
        );
    }
}
