using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AzureFunctions.Adapters;
using Entra.EventHandlers.AzureFunctions.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.DI;

public static class EntraEventHandlersFunctionExtensions
{
    public static IServiceCollection AddEntraEventHandlersForFunctions(this IServiceCollection services)
    {
        services.AddSingleton<IHttpRequestAdapter, HttpRequestAdapter>();
        services.AddSingleton<IHttpResponseAdapter, HttpResponseAdapter>();
        services.AddSingleton<IEntraEventHandlerResolver, EntraEventHandlerResolver>();

        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo(typeof(IEntraEventHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
