using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines a handler for the VerifiedIdClaimValidation event.
/// Implementations process the incoming event and produce a valid response
/// according to the Microsoft Entra custom extension contract.
/// </summary>
/// <remarks>
/// The VerifiedIdClaimValidation event is triggered during the account
/// recovery flow when a user presents a Verified ID credential. A custom
/// extension may validate the claims contained in the credential and decide
/// whether the authentication process should continue or be blocked.
///
/// Handlers may enforce business rules, compare verified claims with internal
/// records, or integrate with external systems to determine the validity of
/// the credential.
///
/// For details on the VerifiedIdClaimValidation event and the expected
/// response schema, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-custom-authentication-extension-account-recovery
/// </remarks>
public interface IVerifiedIdClaimValidationHandler
    : IEntraEventHandler<VerifiedIdClaimValidationEvent, VerifiedIdClaimValidationResponse>
{
}
