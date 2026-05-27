using Entra.EventHandlers.Abstractions.Interfaces;
using System.Text.Json.Serialization;

namespace Entra.EventHandlers.Abstractions.Actions;

[JsonDerivedType(typeof(ContinueAction))]
[JsonDerivedType(typeof(ShowBlockPageAction))]
[JsonDerivedType(typeof(SetPrefillValuesAction))]
[JsonDerivedType(typeof(ModifyAttributeValuesAction))]
[JsonDerivedType(typeof(ShowValidationErrorAction))]
[JsonDerivedType(typeof(ProvideClaimsForTokenAction))]
public abstract class EntraAction : IHaveOdataType
{
    [JsonIgnore]
    public abstract string OdataType { get; }
}
