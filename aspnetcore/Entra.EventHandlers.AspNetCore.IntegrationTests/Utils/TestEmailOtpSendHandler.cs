using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;

namespace Entra.EventHandlers.AspNetCore.IntegrationTests.Utils;

public class TestEmailOtpSendHandler : TestHandlerBase, IEmailOtpSendHandler
{
    public Task<EmailOtpSendResponse> Handle(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken = default)
    {
        CapturedCancellationToken = cancellationToken;

        return Task.FromResult(
            EntraEventResponses
                .EmailOtpSend()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
