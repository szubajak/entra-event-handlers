using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Responses;

namespace Entra.EventHandlers.Abstractions.Interfaces;

/// <summary>
/// Defines a handler for the AttributeCollectionSubmit event.
/// Implementations process the submitted attribute values and produce a valid
/// response according to the Microsoft Entra custom extension contract.
/// </summary>
/// <remarks>
/// The AttributeCollectionSubmit event is triggered after the user provides
/// attribute values during the sign-up or sign-in flow. Handlers may validate
/// the submitted data, transform values, or block the flow.
///
/// For details on the AttributeCollectionSubmit event and the expected response
/// schema, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-onattributecollectionsubmit-retrieve-return-data
/// </remarks>
public interface IAttributeCollectionSubmitHandler 
    : IEntraEventHandler<AttributeCollectionSubmitEvent, AttributeCollectionSubmitResponse>
{
}
