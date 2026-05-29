using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders.Interfaces;

namespace Entra.EventHandlers.Builders.ResponseBuilders;

public class TokenIssuanceStartResponseBuilder : ITokenIssuanceStartResponseBuilderStart, ITokenIssuanceStartResponseBuilderFinal
{
    private EntraAction? _action;

    public ITokenIssuanceStartResponseBuilderFinal ProvideClaimsForToken(Dictionary<string, object> claims)
    {
        _action = new ProvideClaimsForTokenAction
        {
            Claims = claims
        };

        return this;
    }

    public TokenIssuanceStartResponse Build() => new()
    {
        Data = new()
        {
            Actions = [_action!]
        }
    };
}
