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
    /// The OData type for the action that migrates the user’s password to a
    /// new system.
    /// </summary>
    public static readonly PasswordSubmitActionType MigratePassword =
        new(PasswordSubmit.MigratePassword);

    /// <summary>
    /// The OData type for the action that indicates the submitted password is
    /// correct but weak, and instructs the user to reset their password.
    public static readonly PasswordSubmitActionType UpdatePassword =
        new(PasswordSubmit.UpdatePassword);

    /// <summary>
    /// The OData type for the action that instructs the user to retry
    /// password submission.
    /// </summary>
    public static readonly PasswordSubmitActionType Retry =
        new(PasswordSubmit.Retry);

    /// <summary>
    /// The OData type for the action that blocks the password submission
    /// attempt.
    /// </summary>
    public static readonly PasswordSubmitActionType Block =
        new(PasswordSubmit.Block);
}

