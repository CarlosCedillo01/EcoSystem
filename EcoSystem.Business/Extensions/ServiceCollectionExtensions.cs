using EcoSystem.Business.Interfaces;
using EcoSystem.Business.Services;
using EcoSystem.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcoSystem.Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEcoSystemBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEcoSystemData(configuration);

        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IOrdenService, OrdenService>();

        return services;
    }
}
