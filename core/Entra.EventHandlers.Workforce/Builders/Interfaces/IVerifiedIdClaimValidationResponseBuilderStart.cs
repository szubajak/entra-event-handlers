namespace Entra.EventHandlers.Workforce.Builders.Interfaces;

/// <summary>
/// Defines the initial stage of the response builder for the
/// VerifiedIdClaimValidation event. This stage exposes all valid
/// actions that can be returned to Microsoft Entra during the
/// claim validation flow.
/// </summary>
public interface IVerifiedIdClaimValidationResponseBuilderStart
{
    /// <summary>
    /// Returns a response indicating that all verified ID claims
    /// passed validation and the flow may continue.
    /// </summary>
    IVerifiedIdClaimValidationResponseBuilderFinal Pass();

    /// <summary>
    /// Returns a response indicating that one or more verified ID
    /// claims failed validation.
    /// </summary>
    IVerifiedIdClaimValidationResponseBuilderFinal Failed(IEnumerable<string> failedClaims);

    /// <summary>
    /// Begins a fluent builder for specifying failed claims one
    /// entry at a time.
    /// </summary>
    IFailedClaimsBuilder Failed();
}

