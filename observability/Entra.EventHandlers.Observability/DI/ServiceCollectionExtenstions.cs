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
        services.AddScoped<EventLogContext>();
        services.AddScoped<IEventLogWriter, EventLogWriter>();
        services.AddSingleton<IEventLogPublisher, EventLogPublisher>();
        services.AddSingleton<IObservabilityApiClient, ObservabilityApiClient>();

        services.AddSingleton<IEventLogMapperFactory, EventLogMapperFactory>();

        services.AddSingleton<IEmailOtpSendEventLogMapper, EmailOtpSendEventLogMapper>();
        services.AddSingleton<IEventLogMapper<EmailOtpSendEvent, EmailOtpSendResponse>>(sp => sp.GetRequiredService<IEmailOtpSendEventLogMapper>());

        services.Decorate(typeof(IEntraEventHandler<,>), typeof(ObservabilityHandlerDecorator<,>));

        return services;
    }
}
