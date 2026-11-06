using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class PasswordSubmitEndpoint(
    ILogger<PasswordSubmitEndpoint> logger,
    IHttpAdapter httpAdapter,
    IEntraEventHandlerResolver resolver
) : EntraTypedEndpointBase<PasswordSubmitEvent, PasswordSubmitResponse>(logger, httpAdapter, resolver)
{
    protected override string Route => "passwordsubmit";
}
