using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace Sample.Common.Handlers;

public class TokenIssuanceStartHandler(ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCore(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken)
    {
        // Extract user ID (GUID)
        var userId = request.Data.AuthenticationContext?.User?.Id;

        // Example: determine roles based on user ID
        var roles = userId switch
        {
            // Example: special admin GUID
            var id when id == Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                => ["Admin", "PowerUser"],

            // Default
            _ => new[] { "User" }
        };

        // Example: add custom claims
        var customClaims = new Dictionary<string, object>
        {
            { "tenantId", "contoso-eu" },
            { "department", "Engineering" },
            { "roles", roles }
        };

        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken(customClaims)
                .Build());
    }
}