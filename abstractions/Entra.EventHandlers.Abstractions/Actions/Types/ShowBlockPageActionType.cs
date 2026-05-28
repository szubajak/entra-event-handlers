using static Entra.EventHandlers.Abstractions.Protocol.EntraOdataTypes;

namespace Entra.EventHandlers.Abstractions.Actions.Types;

/// <summary>
/// Represents the OData type identifier for a <see cref="ShowBlockPageAction"/>.
/// Each static instance corresponds to the correct action type for a specific
/// event context.
/// </summary>
/// <remarks>
/// Microsoft Entra uses different <c>@odata.type</c> values depending on whether
/// the block page is returned during AttributeCollectionStart or
/// AttributeCollectionSubmit. This type provides strongly typed access to those
/// values.
/// </remarks>
public sealed record ShowBlockPageActionType(string Value)
{
    /// <summary>
    /// Gets the OData type for a block page action returned during the
    /// AttributeCollectionStart event.
    /// </summary>
    public static readonly ShowBlockPageActionType AttributeCollectionStartShowBlockPage =
        new(AttributeCollectionStart.ShowBlockPage);

    /// <summary>
    /// Gets the OData type for a block page action returned during the
    /// AttributeCollectionSubmit event.
    /// </summary>
    public static readonly ShowBlockPageActionType AttributeCollectionSubmitShowBlockPage =
        new(AttributeCollectionSubmit.ShowBlockPage);
}
