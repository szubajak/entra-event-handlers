using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.Hosting.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntraEventHandlers(this IServiceCollection services)
    {
        services.AddEntraEventHandlersHosting();

        services.AddSingleton<IRequestAdapter, RequestAdapter>()
                .AddSingleton<IResponseAdapter, ResponseAdapter>();

        return services;
    }
}
