using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Entra.EventHandlers.UnitTests.Handlers.Base;

public class TestEmailOtpSendHandler(ILogger<EmailOtpSendBase> logger)
    : EmailOtpSendBase(logger)
{
    public int HandleCoreCallCount { get; private set; }

    public CancellationToken? PassedCancellationToken { get; private set; }

    public EmailOtpSendResponse ResponseToReturn { get; set; } = new();

    public bool ThrowOnHandleCore { get; set; } = false;

    protected override Task<EmailOtpSendResponse> HandleCore(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken)
    {
        HandleCoreCallCount++;
        PassedCancellationToken = cancellationToken;

        if (ThrowOnHandleCore) throw new Exception();

        return Task.FromResult(ResponseToReturn);
    }
}
