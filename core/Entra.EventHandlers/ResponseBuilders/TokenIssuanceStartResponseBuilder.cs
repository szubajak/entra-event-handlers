using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.ResponseBuilders;

public interface ITokenIssuanceStartResponseBuilderStart
{
    ITokenIssuanceStartResponseBuilderFinal ProvideClaimsForToken(Dictionary<string, object> claims);
}

public interface ITokenIssuanceStartResponseBuilderFinal
{
    TokenIssuanceStartResponse Build();
}

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
