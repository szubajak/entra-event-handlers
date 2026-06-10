using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders.Interfaces;

namespace Entra.EventHandlers.Builders.ResponseBuilders;

/// <summary>
/// Concrete implementation of the response builder for the
/// TokenIssuanceStart event. This builder enforces the
/// valid action set for this event and produces a fully
/// constructed <see cref="TokenIssuanceStartResponse"/>.
/// </summary>
public sealed class TokenIssuanceStartResponseBuilder : ITokenIssuanceStartResponseBuilderStart, ITokenIssuanceStartResponseBuilderFinal
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

    /// <summary>
    /// Builds the <see cref="TokenIssuanceStartResponse"/> instance
    /// representing the configured action.
    /// </summary>
    /// <remarks>
    /// The <c>_action</c> field is guaranteed to be non-null because
    /// the builder API ensures that exactly one action is selected
    /// before <see cref="Build"/> can be called.
    /// </remarks>
    public TokenIssuanceStartResponse Build() => new()
    {
        Data = new()
        {
            Actions = [_action!]
        }
    };
}
