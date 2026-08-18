using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Hosting.Orchestrators;
using Entra.EventHandlers.Hosting.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.Hosting.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntraEventHandlersHosting(this IServiceCollection services)
    {
        services.AddSingleton<IEntraEventHandlerResolver, EntraEventHandlerResolver>()
                .AddSingleton<IEntraEventOrchestrator, EntraEventOrchestrator>();

        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo(typeof(IEntraEventHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
