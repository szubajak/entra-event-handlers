using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.AspNetCore.Abstractions;
using Entra.EventHandlers.AspNetCore.Adapters;
using Entra.EventHandlers.Hosting.Resolvers;

namespace Entra.EventHandlers.AspNetCore.Endpoints;

public sealed class EmailOtpSendEndpoint(
    ILogger<EmailOtpSendEndpoint> logger,
    IHttpAdapter httpAdapter,
    IEntraEventHandlerResolver resolver
) : EntraTypedEndpointBase<EmailOtpSendEvent, EmailOtpSendResponse>(logger, httpAdapter, resolver)
{
    protected override string Route => "emailotpsend";
}
