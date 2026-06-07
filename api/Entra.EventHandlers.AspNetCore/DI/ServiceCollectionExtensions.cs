using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.AspNetCore.Endpoints;
using Entra.EventHandlers.Hosting.DI;

namespace Entra.EventHandlers.AspNetCore.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntraEventHandlers(this IServiceCollection services)
    {
        services.AddEntraEventHandlersHosting();

        services.AddSingleton<IRequestAdapter, RequestAdapter>();
        services.AddSingleton<IResponseAdapter, ResponseAdapter>();

        services.AddTransient<TokenIssuanceStartEndpoint>();
        services.AddTransient<EntraEventRouterEndpoint>();

        return services;
    }
}
