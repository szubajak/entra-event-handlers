# Entra.EventHandlers.Abstractions

MIT-licensed public contracts for building Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers in .NET.

This package provides the strongly typed protocol models, action definitions, handler interfaces, and shared primitives that form the foundation of the Entra.EventHandlers ecosystem.

## Installation

```bash
dotnet add package Entra.EventHandlers.Abstractions
```

## Features

- Strongly typed event models
- Strongly typed response models
- Action definitions
- Handler interfaces
- Protocol constants
- OData payload models
- Shared protocol primitives
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

- Event models
- Response models
- Action definitions
- Payload types
- Handler interfaces

## Example

```csharp
public interface IAttributeCollectionStartHandler
    : IEntraEventHandler<
        AttributeCollectionStartEvent,
        AttributeCollectionStartResponse>
{
}
```

## Why This Package Exists

The Entra.EventHandlers ecosystem is divided into contracts and implementations.

This package contains the stable public protocol contracts used by applications, libraries, hosting adapters, and testing frameworks without requiring any implementation dependencies.

Benefits include:

- Stable API surface
- Framework-independent contracts
- Easier testing
- Reusable integrations
- Long-term compatibility
- Clear separation of concerns

### Not Included

This package intentionally does not include:

- Hosting integrations
- Dependency injection
- Logging
- Validation pipelines
- Event orchestration
- Response builders
- Handler base classes

These capabilities are provided by the implementation and hosting packages.

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting adapter |
| Entra.EventHandlers.AzureFunctions | Azure Functions hosting adapter |
| Entra.EventHandlers.Security | PasswordSubmit decryption and Azure Key Vault integration |

## Documentation

Documentation, hosting guides, event references, architecture documentation, and samples:

https://github.com/szubajak/entra-event-handlers/tree/main/docs

AI-friendly repository metadata:

https://github.com/szubajak/entra-event-handlers/blob/main/llms.txt

## License

This package is licensed under the MIT License.

The implementation, hosting, and security packages in the Entra.EventHandlers ecosystem are licensed separately under the Business Source License (BSL).

See the repository for complete licensing details.

## Further Reading

**Entra External ID .NET Handlers Deep Dive**

https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437

**Building CIAM-Ready Azure Functions with Entra.EventHandlers**

https://medium.com/@jakub.szubarga/entra-eventhandlers-ciam-azure-functions-97c5e1940272