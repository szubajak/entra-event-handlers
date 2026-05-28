using Entra.EventHandlers.Abstractions.Actions;
using Entra.EventHandlers.Abstractions.Interfaces;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Responses;

/// <summary>
/// Base type for all Microsoft Entra custom extension response payloads.
/// Contains the required <c>@odata.type</c> discriminator and the collection
/// of actions that instruct Entra how to proceed with the authentication flow.
/// </summary>
/// <remarks>
/// Derived response payloads specify the concrete <c>@odata.type</c> value
/// expected by the Entra protocol and populate the <c>actions</c> collection
/// with one or more instructions such as validation results, page redirects,
/// or claim modifications.
///
/// For details on response schemas and supported actions, see:
/// https://learn.microsoft.com/en-us/entra/identity-platform/custom-extension-overview
/// </remarks>
public abstract class EntraEventResponsePayload : IHaveOdataType
{
    /// <summary>
    /// Gets the expected <c>@odata.type</c> discriminator for the response
    /// payload. Derived types override this value to match the Entra protocol
    /// contract.
    /// </summary>
    [JsonIgnore]
    public abstract string OdataType { get; }

    /// <summary>
    /// Gets or sets the collection of actions that Microsoft Entra should
    /// perform in response to the event. Each action represents a specific
    /// instruction defined by the custom extension contract.
    /// </summary>
    [JsonPropertyName("actions")]
    public IEnumerable<EntraAction> Actions { get; set; } = [];
}