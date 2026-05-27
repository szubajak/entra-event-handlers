using static Entra.EventHandlers.Abstractions.Protocol.EntraOdataTypes;

namespace Entra.EventHandlers.Abstractions.Actions.Types;

public sealed record ShowBlockPageActionType(string Value)
{
    public static readonly ShowBlockPageActionType AttributeCollectionStartShowBlockPage =
        new(AttributeCollectionStart.ShowBlockPage);

    public static readonly ShowBlockPageActionType AttributeCollectionSubmitShowBlockPage =
        new(AttributeCollectionSubmit.ShowBlockPage);
}
