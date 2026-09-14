using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Factories;
using Entra.EventHandlers.Observability.Logging;

namespace Entra.EventHandlers.Observability.Decorators;

public sealed class ObservabilityHandlerDecorator<TRequest, TResponse>(
    IEntraEventHandler<TRequest, TResponse> inner,
    IEventLogPublisher publisher,
    IEventLogMapperFactory mapperFactory,
    EventLogContext ctx) 
    : IEntraEventHandler<TRequest, TResponse>
    where TRequest : EntraEvent
    where TResponse : EntraEventResponse
{
    private readonly IEntraEventHandler<TRequest, TResponse> _inner = inner;
    private readonly IEventLogPublisher _publisher = publisher;
    private readonly IEventLogMapperFactory _mapperFactory = mapperFactory;
    private readonly EventLogContext _ctx = ctx;

    public async Task<EntraHandlerResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _inner.HandleAsync(request, cancellationToken);

        if (result.HasException)
            return result;

        var mapper = _mapperFactory.Get<TRequest, TResponse>();

        _ctx.DefaultLog = mapper.Map(request, result.Response);

        _publisher.Publish(_ctx);

        return result;
    }
}
