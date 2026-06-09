using Entra.EventHandlers.Abstractions.Events;
using static Entra.EventHandlers.Abstractions.Protocol.EntraOdataTypes;

namespace Entra.EventHandlers.Abstractions.Actions.Types;

/// <summary>
/// Represents the OData type identifier for a <see cref="ShowBlockPageAction"/>.
/// Each static instance corresponds to the correct action type for a specific
/// event context.
/// </summary>
/// <remarks>
/// Microsoft Entra uses different <c>@odata.type</c> values depending on the
/// event in which a block‑page action is returned. This type provides strongly
/// typed access to those values and ensures correct protocol usage.
/// </remarks>
public sealed record ShowBlockPageActionType(string Value)
{
    /// <summary>
    /// The OData type for the action that displays a block page to the user
    /// during the <see cref="AttributeCollectionStartEvent"/>.
    /// </summary>
    public static readonly ShowBlockPageActionType AttributeCollectionStartShowBlockPage =
        new(AttributeCollectionStart.ShowBlockPage);

    /// <summary>
    /// The OData type for the action that displays a block page to the user
    /// during the <see cref="AttributeCollectionSubmitEvent"/>.
    /// </summary>
    public static readonly ShowBlockPageActionType AttributeCollectionSubmitShowBlockPage =
        new(AttributeCollectionSubmit.ShowBlockPage);
}
