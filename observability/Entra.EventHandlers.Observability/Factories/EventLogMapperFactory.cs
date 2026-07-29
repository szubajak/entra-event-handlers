using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Observability.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.EventHandlers.Observability.Factories;

public interface IEventLogMapperFactory
{
    IEventLogMapper<TRequest, TResponse> Get<TRequest, TResponse>()
        where TRequest : EntraEvent
        where TResponse : EntraEventResponse;
}

public sealed class EventLogMapperFactory(IServiceProvider serviceProvider)
    : IEventLogMapperFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IEventLogMapper<TRequest, TResponse> Get<TRequest, TResponse>()
       where TRequest : EntraEvent
       where TResponse : EntraEventResponse
    {
        var mapper = _serviceProvider.GetRequiredService<IEventLogMapper<TRequest, TResponse>>();

        return mapper is null
            ? throw new InvalidOperationException($"No mapper registered for {typeof(TRequest).Name} → {typeof(TResponse).Name}")
            : mapper;
    }
}
