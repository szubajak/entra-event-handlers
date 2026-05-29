namespace Entra.EventHandlers.Builders.Interfaces;

public interface ITokenIssuanceStartResponseBuilderStart
{
    ITokenIssuanceStartResponseBuilderFinal ProvideClaimsForToken(Dictionary<string, object> claims);
}