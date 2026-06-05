using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.DI;

public static class EntraEventHandlersFunctionsExtensions
{
    public static IServiceCollection AddEntraEventHandlersForFunctions(this IServiceCollection services)
    {
        services.AddEntraEventHandlersHosting();

        services.AddSingleton<IAzureFunctionsRequestAdapter, AzureFunctionsRequestAdapter>();
        services.AddSingleton<IAzureFunctionsResponseAdapter, AzureFunctionsResponseAdapter>();

        return services;
    }
}
