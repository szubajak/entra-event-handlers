using static Entra.EventHandlers.Abstractions.Protocol.EntraOdataTypes;

namespace Entra.EventHandlers.Abstractions.Actions.Types;

/// <summary>
/// Represents the OData type identifier for a <see cref="ContinueAction"/>.
/// Each static instance corresponds to the correct action type for a specific
/// event context.
/// </summary>
/// <remarks>
/// Microsoft Entra uses different <c>@odata.type</c> values depending on whether
/// the continue-with-default-behavior action is returned during
/// AttributeCollectionStart or AttributeCollectionSubmit. This type provides
/// strongly typed access to those values and ensures correct protocol usage.
/// </remarks>
public sealed record ContinueActionType(string Value)
{
    /// <summary>
    /// Gets the OData type for a continue-with-default-behavior action returned
    /// during the AttributeCollectionStart event.
    /// </summary>
    public static readonly ContinueActionType AttributeCollectionStartContinueWithDefaultBehavior =
        new(AttributeCollectionStart.ContinueWithDefaultBehavior);

    /// <summary>
    /// Gets the OData type for a continue-with-default-behavior action returned
    /// during the AttributeCollectionSubmit event.
    /// </summary>
    public static readonly ContinueActionType AttributeCollectionSubmitContinueWithDefaultBehavior =
        new(AttributeCollectionSubmit.ContinueWithDefaultBehavior);
}