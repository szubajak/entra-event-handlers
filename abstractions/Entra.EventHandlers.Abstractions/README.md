# Entra.EventHandlers.Abstractions

MIT-licensed public contracts for building Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers in .NET.

This package provides the strongly typed protocol models, action definitions, interfaces, and shared primitives used throughout the Entra.EventHandlers ecosystem.

## Installation

```bash
dotnet add package Entra.EventHandlers.Abstractions
```

## Features

- Strongly typed event request models
- Strongly typed response models
- Action definitions
- Handler interfaces
- Protocol constants
- OData payload models
- Dependency-free design
- Full XML documentation

## Supported Events

### External ID

- AttributeCollectionStart
- AttributeCollectionSubmit
- EmailOtpSend
- PasswordSubmit
- TokenIssuanceStart

### Workforce

- VerifiedIdClaimValidation

Each event includes:

- Request model
- Response model
- Action definitions
- Payload types
- Handler interfaces

## Example

```csharp
public interface IAttributeCollectionStartHandler
    : IEntraEventHandler<AttributeCollectionStartEvent, AttributeCollectionStartResponse>
{
}
```

## Why This Package Exists

The abstractions package separates the public protocol contract from the implementation layer.

Benefits include:

- Stable API surface
- Framework-independent contracts
- Easier testing
- Reusable integrations
- Long-term compatibility

This package contains no:

- Hosting integrations
- Dependency injection setup
- Logging
- Validation pipelines
- Response builders

Those capabilities are provided by the implementation packages.

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting |
| Entra.EventHandlers.AzureFunctions | Azure Functions hosting |
| Entra.EventHandlers.Security | PasswordSubmit decryption and Azure Key Vault integration |

## Documentation

Full documentation, event guides, hosting guides, and samples:

https://github.com/szubajak/entra-eventhandlers/tree/main/docs

AI-friendly repository metadata:

https://github.com/szubajak/entra-eventhandlers/blob/main/llms.txt

## License

This package is licensed under the MIT License.

The implementation and hosting packages use the Business Source License (BSL).

## Further Reading

Entra External ID .NET Handlers Deep Dive

https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437