using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Workforce.Builders.Interfaces;

/// <summary>
/// Represents the final stage of the response builder for the
/// VerifiedIdClaimValidation event. At this stage all action
/// configuration is complete and the response object can be
/// constructed.
/// </summary>
public interface IVerifiedIdClaimValidationResponseBuilderFinal
{
    /// <summary>
    /// Builds the <see cref="VerifiedIdClaimValidationResponse"/> instance
    /// representing the configured action.
    /// </summary>
    VerifiedIdClaimValidationResponse Build();
}
