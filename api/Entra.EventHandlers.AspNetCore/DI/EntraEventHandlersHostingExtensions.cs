using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AspNetCore.DI;

public static class EntraEventHandlersAspNetCoreExtensions
{
    public static IServiceCollection AddEntraEventHandlersForAspNetCore(this IServiceCollection services)
    {
        services.AddEntraEventHandlersHosting();

        services.AddSingleton<IAspNetCoreRequestAdapter, AspNetCoreRequestAdapter>();
        services.AddSingleton<IAspNetCoreResponseAdapter, AspNetCoreResponseAdapter>();

        return services;
    }
}
