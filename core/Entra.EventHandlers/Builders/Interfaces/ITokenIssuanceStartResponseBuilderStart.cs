namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Defines the initial stage of the response builder for the
/// TokenIssuanceStart event. This stage exposes all valid actions
/// that can be returned to Microsoft Entra during token issuance.
/// </summary>
public interface ITokenIssuanceStartResponseBuilderStart
{
    /// <summary>
    /// Returns a response that provides additional claims to be
    /// included in the issued token. The supplied dictionary maps
    /// claim names to their corresponding values.
    /// </summary>
    ITokenIssuanceStartResponseBuilderFinal ProvideClaimsForToken(Dictionary<string, object> claims);
}