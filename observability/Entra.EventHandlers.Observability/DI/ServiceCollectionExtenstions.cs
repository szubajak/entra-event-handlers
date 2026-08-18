using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Observability.Clients;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Decorators;
using Entra.EventHandlers.Observability.Factories;
using Entra.EventHandlers.Observability.Interfaces;
using Entra.EventHandlers.Observability.Logging;
using Entra.EventHandlers.Observability.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.Observability.DI;

public static class ServiceCollectionExtenstions
{
    public static IServiceCollection AddEntraEventHandlersObservability(this IServiceCollection services)
    {
        services.AddScoped<EventLogContext>()
                .AddScoped<IEventLogWriter, EventLogWriter>()
                .AddSingleton<IEventLogPublisher, EventLogPublisher>()
                .AddSingleton<IObservabilityApiClient, ObservabilityApiClient>()
                .AddSingleton<IEventLogMapperFactory, EventLogMapperFactory>()
                .AddSingleton<IEventLogContextMapper, EventLogContextMapper>()
                .AddSingleton<IEmailOtpSendEventLogMapper, EmailOtpSendEventLogMapper>()
                .AddSingleton<IEventLogMapper<EmailOtpSendEvent, EmailOtpSendResponse>>(sp => sp.GetRequiredService<IEmailOtpSendEventLogMapper>());

        var hasHandlers = services.Any(sd =>
            sd.ServiceType.IsGenericType &&
            sd.ServiceType.GetGenericTypeDefinition() == typeof(IEntraEventHandler<,>)
        );

        if (hasHandlers)
        {
            services.Decorate(typeof(IEntraEventHandler<,>), typeof(ObservabilityHandlerDecorator<,>));
        }

        return services;
    }
}
