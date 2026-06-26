namespace Entra.EventHandlers.Workforce.Builders.Interfaces;

/// <summary>
/// Fluent builder for constructing the list of failed verified ID
/// claims one entry at a time.
/// </summary>
public interface IFailedClaimsBuilder
{
    /// <summary>
    /// Adds a failed claim name to the list.
    /// </summary>
    IFailedClaimsBuilder Add(string claimName);

    /// <summary>
    /// Completes the failed-claims configuration and returns the
    /// final stage of the response builder.
    /// </summary>
    IVerifiedIdClaimValidationResponseBuilderFinal Done();
}
