using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Sample.Common.Handlers;

public class EmailOtpSendHandler(ILogger<EmailOtpSendHandler> logger)
    : EmailOtpSendHandlerBase(logger)
{
    protected override Task<EmailOtpSendResponse> HandleCoreAsync(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            EntraEventResponses
                .EmailOtpSend()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}