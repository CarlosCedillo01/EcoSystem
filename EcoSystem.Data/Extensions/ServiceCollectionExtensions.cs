using EcoSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcoSystem.Data.Extensions;

/// <summary>
/// Extensiones para registrar los servicios de la capa de datos en el contenedor de inyección de dependencias.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra el EcoSystemDbContext con la cadena de conexión "DefaultConnection" de la configuración.
    /// </summary>
    public static IServiceCollection AddEcoSystemData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EcoSystemDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(EcoSystemDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                }));

        return services;
    }
}
