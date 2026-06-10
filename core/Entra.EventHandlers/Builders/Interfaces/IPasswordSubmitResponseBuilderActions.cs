namespace Entra.EventHandlers.Builders.Interfaces;

/// <summary>
/// Represents the action-selection stage of the response builder for the
/// PasswordSubmit event. After the nonce is provided, this stage exposes
/// all valid actions that can be returned to Microsoft Entra during the
/// password migration flow.
/// </summary>
public interface IPasswordSubmitResponseBuilderActions
{
    /// <summary>
    /// Returns a response indicating that the submitted password is valid.
    /// Entra continues authentication, and the external system should
    /// migrate the password and clear the migration flag.
    /// </summary>
    /// <remarks>
    /// Use this action when the password meets validation requirements.
    /// If the password is weak, Entra may still trigger the
    /// <see cref="UpdatePassword"/> flow after migration.
    /// </remarks>
    IPasswordSubmitResponseBuilderFinal MigratePassword();

    /// <summary>
    /// Returns a response indicating that the submitted password is correct
    /// but weak or expired. Entra routes the user through the password reset
    /// flow to create a stronger password.
    /// </summary>
    /// <remarks>
    /// Use this action when the password is valid but does not meet strength
    /// requirements or is expired.
    /// </remarks>
    IPasswordSubmitResponseBuilderFinal UpdatePassword();

    /// <summary>
    /// Returns a response indicating that the submitted password is
    /// incorrect. Entra allows the user to retry authentication if permitted.
    /// </summary>
    /// <remarks>
    /// Use this action when password validation fails but the user should be
    /// allowed another attempt.
    /// </remarks>
    IPasswordSubmitResponseBuilderFinal Retry();

    /// <summary>
    /// Returns a response instructing Entra to block the authentication
    /// attempt. Entra displays a block screen with a custom message provided
    /// by the application.
    /// </summary>
    /// <remarks>
    /// Use this action when authentication must not proceed, such as when
    /// the account is locked or access is denied by the legacy system.
    /// </remarks>
    IPasswordSubmitResponseBuilderFinal Block();
}
