using Entra.EventHandlers.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.AzureFunctions.DI;

public static class EntraEventHandlersFunctionExtensions
{
    public static IServiceCollection AddEntraEventHandlersForFunctions(this IServiceCollection services)
    {
        var handlerTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntraEventHandler<,>)));

        foreach (var type in handlerTypes)
        {
            var iface = type.GetInterfaces().First(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntraEventHandler<,>));

            services.AddTransient(typeof(IEntraEventHandler), type);
            services.AddTransient(iface, type);
        }

        return services;
    }
}
