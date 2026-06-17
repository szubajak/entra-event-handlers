using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.TestHelpers;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestRequestAdapter : IRequestAdapter
{
    public Task<TEvent> ReadEvent<TEvent>(HttpContext context)
        where TEvent : EntraEvent =>
        Task.FromResult(Activator.CreateInstance<TEvent>());

    public Task<EntraEvent> ReadEvent(HttpContext context) =>
        Task.FromResult<EntraEvent>(new TestEvent());
}
