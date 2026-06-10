using Entra.EventHandlers.Abstractions.Events;
using static Entra.EventHandlers.Abstractions.Protocol.EntraOdataTypes;

namespace Entra.EventHandlers.Abstractions.Actions.Types;

/// <summary>
/// Represents the OData type identifier for a <see cref="PasswordSubmitAction"/>.
/// Each static instance corresponds to one of the supported actions that can be
/// returned during a <see cref="PasswordSubmitEvent"/>.
/// </summary>
/// <remarks>
/// Microsoft Entra defines four distinct actions for the PasswordSubmit event:
/// migrate password, update password, retry, and block. Each action has its own
/// <c>@odata.type</c> discriminator. This type provides strongly typed access to
/// those identifiers and ensures correct protocol usage.
/// </remarks>
public sealed record PasswordSubmitActionType(string Value)
{
    /// <summary>
    /// The OData type for the action that indicates the submitted password is
    /// valid. Entra continues authentication and the external system should
    /// migrate the password and clear the migration flag.
    /// </summary>
    /// <remarks>
    /// Use this action when the password meets validation requirements.
    /// If the password is weak, Entra may still trigger the UpdatePassword
    /// flow after migration.
    /// </remarks>
    public static readonly PasswordSubmitActionType MigratePassword =
        new(PasswordSubmit.MigratePassword);

    /// <summary>
    /// The OData type for the action that indicates the submitted password is
    /// correct but weak or expired. Entra routes the user through the password
    /// reset flow to create a stronger password.
    /// </summary>
    /// <remarks>
    /// Use this action when the password is valid but does not meet strength
    /// requirements or is expired.
    /// </remarks>
    public static readonly PasswordSubmitActionType UpdatePassword =
        new(PasswordSubmit.UpdatePassword);

    /// <summary>
    /// The OData type for the action that indicates the submitted password is
    /// incorrect. Entra allows the user to retry authentication if permitted.
    /// </summary>
    /// <remarks>
    /// Use this action when password validation fails but the user should be
    /// allowed another attempt.
    /// </remarks>
    public static readonly PasswordSubmitActionType Retry =
        new(PasswordSubmit.Retry);

    /// <summary>
    /// The OData type for the action that instructs Entra to block the
    /// authentication attempt. Entra displays a block screen with a custom
    /// message provided by the application.
    /// </summary>
    /// <remarks>
    /// Use this action when authentication must not proceed, such as when the
    /// account is locked or access is denied by the legacy system.
    /// </remarks>
    public static readonly PasswordSubmitActionType Block =
        new(PasswordSubmit.Block);

}
