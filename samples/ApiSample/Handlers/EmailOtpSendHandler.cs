using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;

namespace ApiSample.Handlers;

public class EmailOtpSendHandler(ILogger<EmailOtpSendHandler> logger)
    : EmailOtpSendHandlerBase(logger)
{
    protected override Task<EmailOtpSendResponse> HandleCore(
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