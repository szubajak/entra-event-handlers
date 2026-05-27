using static Entra.EventHandlers.Abstractions.Protocol.EntraOdataTypes;

namespace Entra.EventHandlers.Abstractions.Actions.Types;

public sealed record ContinueActionType(string Value)
{
    public static readonly ContinueActionType AttributeCollectionStartContinueWithDefaultBehavior =
        new(AttributeCollectionStart.ContinueWithDefaultBehavior);

    public static readonly ContinueActionType AttributeCollectionSubmitContinueWithDefaultBehavior =
        new(AttributeCollectionSubmit.ContinueWithDefaultBehavior);
}