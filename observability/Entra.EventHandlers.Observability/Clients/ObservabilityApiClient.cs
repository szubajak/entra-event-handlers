using Entra.EventHandlers.Observability.Dtos;
using System.Text;
using System.Text.Json;

namespace Entra.EventHandlers.Observability.Clients;

public interface IObservabilityApiClient
{
    Task SendAsync(EventLogDto eventLogDto);
}

public class ObservabilityApiClient(HttpClient client) : IObservabilityApiClient
{
    private readonly HttpClient _client = client;

    public async Task SendAsync(EventLogDto eventLogDto)
    {
        var json = JsonSerializer.Serialize(eventLogDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await _client.PostAsync("/logs", content);
    }
}
