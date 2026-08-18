using Entra.EventHandlers.Observability.Clients;
using Entra.EventHandlers.Observability.Context;
using Entra.EventHandlers.Observability.Mappers;

namespace Entra.EventHandlers.Observability.Logging;

public interface IEventLogPublisher
{
    void Publish(EventLogContext ctx);
}

public class EventLogPublisher(IObservabilityApiClient client, IEventLogContextMapper mapper)
    : IEventLogPublisher
{
    private readonly IObservabilityApiClient _client = client;

    private readonly IEventLogContextMapper _mapper = mapper;

    public void Publish(EventLogContext ctx)
    {
        var dto = _mapper.Map(ctx);

        _ = Task.Run(async () =>
        {
            await _client.SendAsync(dto);
        });
    }
}
