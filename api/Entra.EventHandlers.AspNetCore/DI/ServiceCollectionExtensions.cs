using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AspNetCore.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntraEventHandlers(this IServiceCollection services)
    {
        services.AddEntraEventHandlersHosting();

        services.AddSingleton<IRequestAdapter, RequestAdapter>();
        services.AddSingleton<IResponseAdapter, ResponseAdapter>();

        return services;
    }
}
