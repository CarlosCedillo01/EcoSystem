using EcoSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoSystem.Data.Context;

/// <summary>
/// Contexto principal de la base de datos de EcoSystem.
/// Conecta con PostgreSQL en Supabase usando Entity Framework Core.
/// </summary>
public class EcoSystemDbContext : DbContext
{
    public EcoSystemDbContext(DbContextOptions<EcoSystemDbContext> options)
        : base(options)
    {
    }

    // DbSets — Tablas de la tienda
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Orden> Ordenes => Set<Orden>();
    public DbSet<DetalleOrden> DetalleOrdenes => Set<DetalleOrden>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura el esquema por defecto de Supabase
        modelBuilder.HasDefaultSchema("public");

        // Aplica todas las configuraciones de entidades desde este ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcoSystemDbContext).Assembly);
    }
}
