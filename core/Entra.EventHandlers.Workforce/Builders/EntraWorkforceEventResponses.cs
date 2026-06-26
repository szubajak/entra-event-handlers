using Entra.EventHandlers.Workforce.Builders.Interfaces;
using Entra.EventHandlers.Workforce.Builders.ResponseBuilders;

namespace Entra.EventHandlers.Workforce.Builders;

/// <summary>
/// Provides entry points for constructing Microsoft Entra Workforce event
/// responses. Each method returns a strongly typed builder for the
/// corresponding Workforce event type.
/// </summary>
/// <remarks>
/// These builders offer a structured, discoverable way to construct responses
/// for Workforce-specific events such as VerifiedIdClaimValidation.
/// </remarks>
public static class EntraWorkforceEventResponses
{
    /// <summary>
    /// Creates a builder for constructing a VerifiedIdClaimValidation response.
    /// </summary>
    public static IVerifiedIdClaimValidationResponseBuilderStart VerifiedIdClaimValidation() =>
        new VerifiedIdClaimValidationResponseBuilder();
}

