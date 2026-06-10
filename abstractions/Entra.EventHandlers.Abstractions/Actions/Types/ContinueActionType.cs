using Entra.EventHandlers.Abstractions.Events;
using static Entra.EventHandlers.Abstractions.Protocol.EntraOdataTypes;

namespace Entra.EventHandlers.Abstractions.Actions.Types;

/// <summary>
/// Represents the OData type identifier for a <see cref="ContinueAction"/>.
/// Each static instance corresponds to the correct action type for a specific
/// event context.
/// </summary>
/// <remarks>
/// Microsoft Entra uses different <c>@odata.type</c> values depending on the
/// event in which a continue‑with‑default‑behavior action is returned.
/// This type provides strongly typed access to those values and ensures
/// correct protocol usage.
/// </remarks>
public sealed record ContinueActionType(string Value)
{
    /// <summary>
    /// The OData type for the action that instructs Entra to continue with its
    /// default behavior during the <see cref="AttributeCollectionStartEvent"/>.
    /// </summary>
    public static readonly ContinueActionType AttributeCollectionStartContinueWithDefaultBehavior =
        new(AttributeCollectionStart.ContinueWithDefaultBehavior);

    /// <summary>
    /// The OData type for the action that instructs Entra to continue with its
    /// default behavior during the <see cref="AttributeCollectionSubmitEvent"/>.
    /// </summary>
    public static readonly ContinueActionType AttributeCollectionSubmitContinueWithDefaultBehavior =
        new(AttributeCollectionSubmit.ContinueWithDefaultBehavior);

    /// <summary>
    /// The OData type for the action that instructs Entra to continue with its
    /// default behavior during the <see cref="EmailOtpSendEvent"/>.
    /// </summary>
    public static readonly ContinueActionType EmailOtpSendContinueWithDefaultBehavior =
        new(EmailOtpSend.ContinueWithDefaultBehavior);
}

