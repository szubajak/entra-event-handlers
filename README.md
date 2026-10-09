# Entra.EventHandlers

[![Coverage](https://szubajak.github.io/entra-event-handlers/badge_shieldsio_branchcoverage_blue.svg)](https://szubajak.github.io/entra-event-handlers/)

A modern .NET ecosystem for building Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers using strongly typed models, fluent response builders, and production-ready hosting integrations.

The ecosystem provides:

- Strongly typed event models
- Strongly typed response models
- Fluent response builders
- Handler base classes
- Dependency injection integration
- ASP.NET Core hosting
- Azure Functions hosting
- PasswordSubmit decryption support
- Unit testing friendly architecture

---

## Packages

### Abstractions (MIT)

Public contracts, protocol models, interfaces, and shared primitives.

[![NuGet](https://img.shields.io/nuget/v/Entra.EventHandlers.Abstractions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Abstractions)
[![Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.Abstractions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Abstractions)

---

### Entra.EventHandlers (BSL)

External ID implementation layer including:

- Handler base classes
- Fluent response builders
- Validation
- Logging
- Execution pipeline

[![NuGet](https://img.shields.io/nuget/v/Entra.EventHandlers.svg)](https://www.nuget.org/packages/Entra.EventHandlers)
[![Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.svg)](https://www.nuget.org/packages/Entra.EventHandlers)

---

### Entra.EventHandlers.Workforce (BSL)

Workforce implementation layer including:

- VerifiedIdClaimValidation
- Workforce handlers
- Workforce response builders

[![NuGet](https://img.shields.io/nuget/v/Entra.EventHandlers.Workforce.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Workforce)
[![Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.Workforce.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Workforce)

---

### Entra.EventHandlers.AspNetCore (BSL)

ASP.NET Core hosting integration.

[![NuGet](https://img.shields.io/nuget/v/Entra.EventHandlers.AspNetCore.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AspNetCore)
[![Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.AspNetCore.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AspNetCore)

---

### Entra.EventHandlers.AzureFunctions (BSL)

Azure Functions hosting integration.

[![NuGet](https://img.shields.io/nuget/v/Entra.EventHandlers.AzureFunctions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AzureFunctions)
[![Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.AzureFunctions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AzureFunctions)

---

### Entra.EventHandlers.Security (BSL)

Security extensions including:

- PasswordSubmit decryption
- Azure Key Vault integration
- Certificate providers
- RSA key extraction

[![NuGet](https://img.shields.io/nuget/v/Entra.EventHandlers.Security.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Security)
[![Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.Security.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Security)

---

## Why Entra.EventHandlers?

Microsoft Entra authentication events are exposed as HTTP-based extensibility points.

Implementing them directly typically requires:

- Request deserialization
- Protocol validation
- Routing
- Logging
- Response construction
- Error handling
- Dependency injection setup

Entra.EventHandlers provides a unified programming model that removes this boilerplate while remaining strongly typed and testable.

---

## Quick Start

Install the core package:

```bash
dotnet add package Entra.EventHandlers
```

Install a hosting integration:

```bash
dotnet add package Entra.EventHandlers.AspNetCore
```

or

```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```

Register services:

```csharp
builder.Services.AddEntraEventHandlers();
```

Implement a handler:

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken(
                    new Dictionary<string, object>
                    {
                        ["department"] = "Engineering"
                    })
                .Build());
    }
}
```

---

## Documentation

### Start Here

- [Getting Started](./docs/getting-started.md)
- [Architecture](./docs/architecture.md)
- [Unit Testing](./docs/testing.md)

### Hosting

- [AspNetCore](./docs/hosting/aspnetcore.md)
- [AzureFunctions](./docs/hosting/azure-functions.md)

### Security

- [EncryptedPasswordContext Decryption](./docs/security/password-submit-decryption.md)

### Events

#### External ID

- [AttributeCollectionStart](./docs/events/attribute-collection-start.md)
- [AttributeCollectionSubmit](./docs/events/attribute-collection-submit.md)
- [AspNetCore](./docs/events/email-otp-send.md)
- [PasswordSubmit](./docs/events/password-submit.md)
- [TokenIssuanceStart](./docs/events/token-issuance-start.md)

#### Workforce

- [VerifiedIdClaimValidation](./docs/events/verified-id-claim-validation.md)

---

## Samples

### ASP.NET Core

- [Api Sample](samples/ApiSample)

### Azure Functions

- [Function App (AzureFunctions) Sample](samples/AzureFunctionsSample)

### Entra Event Handlers

- [Entra Event Handlers Sample](samples/Sample.Common)

### Minimal Azure Functions Sample

- https://github.com/szubajak/entra-event-handlers-azurefunctions

---

## Ecosystem Overview

```text
Entra.EventHandlers Ecosystem

├─ Entra.EventHandlers.Abstractions
│  └─ Public contracts and protocol models
│
├─ Entra.EventHandlers
│  └─ External ID implementation layer
│
├─ Entra.EventHandlers.Workforce
│  └─ Workforce implementation layer
│
├─ Hosting
│  ├─ Entra.EventHandlers.AspNetCore
│  └─ Entra.EventHandlers.AzureFunctions
│
└─ Optional Extensions
   └─ Entra.EventHandlers.Security
```

For detailed architecture documentation see:

- [Architecture](./docs/architecture.md)

---

## AI Discovery

This repository includes AI-friendly documentation and metadata:

- `llms.txt`
- `docs/`

AI assistants should begin with:

1. `llms.txt`
2. `docs/getting-started.md`
3. `docs/architecture.md`

When generating examples, prefer:

- Strongly typed handlers
- Fluent response builders
- `AddEntraEventHandlers()`
- Router-based hosting
- Direct handler unit testing

---

## Licensing

### MIT

- Entra.EventHandlers.Abstractions

### Business Source License (BSL)

- Entra.EventHandlers
- Entra.EventHandlers.Workforce
- Entra.EventHandlers.AspNetCore
- Entra.EventHandlers.AzureFunctions
- Entra.EventHandlers.Security

See the individual package licenses for details.

---

## Commercial Licensing

A commercial license covers the entire Entra.EventHandlers ecosystem, including all current and future BSL-licensed packages.

For commercial licensing and support:

**jakub.szubarga@gmail.com**

---

## Contributing

Contributions are welcome for the MIT-licensed abstractions package.

For implementation packages, issues, discussions, bug reports, feature requests, and documentation improvements are always appreciated.

---

## Author

**Jakub Szubarga**

If this project helps you, consider starring the repository.