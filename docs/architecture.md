# Architecture

This document explains the architectural design of the Entra.EventHandlers ecosystem and how the individual packages work together.

The ecosystem is designed to provide strongly typed Microsoft Entra authentication event handling while keeping business logic independent from hosting infrastructure.

---

## Design Goals

Entra.EventHandlers was designed with the following goals:

- Strongly typed Microsoft Entra event models
- Separation between business logic and hosting infrastructure
- Reusable handlers across hosting platforms
- Dependency injection friendly APIs
- Consistent developer experience
- Testable application code
- Minimal boilerplate
- Forward compatibility as Microsoft Entra evolves

---

# Ecosystem Overview

The Entra.EventHandlers ecosystem is organized into several packages with clearly defined responsibilities.

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

---

# Package Responsibilities

| Package | Responsibility |
|----------|----------|
| Entra.EventHandlers.Abstractions | Protocol contracts, models, interfaces, and shared primitives |
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting |
| Entra.EventHandlers.AzureFunctions | Azure Functions hosting |
| Entra.EventHandlers.Security | Optional security and cryptography helpers |

---

# Package Dependencies

The packages build upon one another while maintaining clear separation of responsibilities.

```text
Entra.EventHandlers.Abstractions
                ▲
                │
      ┌─────────┴─────────┐
      │                   │
      ▼                   ▼
Entra.EventHandlers   Entra.EventHandlers.Workforce
      ▲                   ▲
      │                   │
      └─────────┬─────────┘
                │
      ┌─────────┴─────────┐
      ▼                   ▼
Entra.EventHandlers  Entra.EventHandlers
.AspNetCore          .AzureFunctions


Entra.EventHandlers.Security

(Optional extension package used
primarily for PasswordSubmit and
Azure Key Vault scenarios)
```

---

# Entra.EventHandlers.Abstractions

The abstractions package defines the public protocol contract.

Contents:

- Event models
- Response models
- Action models
- Handler interfaces
- Protocol constants
- Shared primitives

Design goals:

- Stable public API
- Minimal dependencies
- Reusable across projects
- MIT licensing

This package does not contain:

- Hosting integrations
- Dependency injection
- Logging
- Validation
- Response builders
- Business logic

Example:

```csharp
public interface IAttributeCollectionStartHandler
    : IEntraEventHandler<
        AttributeCollectionStartEvent,
        AttributeCollectionStartResponse>
{
}
```

---

# Entra.EventHandlers

The core package provides the implementation layer for Microsoft Entra External ID authentication events.

Contents:

- Handler base classes
- Fluent response builders
- Validation
- Logging
- Correlation tracking
- Execution timing
- Default exception handling

Example:

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
}
```

Most External ID applications depend on this package.

---

# Entra.EventHandlers.Workforce

Provides support for Microsoft Entra Workforce authentication events.

Contents:

- Workforce event models
- Workforce handler base classes
- Workforce response builders
- Workforce-specific protocol support

Example:

```csharp
public class VerifiedIdClaimValidationHandler(
    ILogger<VerifiedIdClaimValidationHandler> logger)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
}
```

This package follows the same programming model as Entra.EventHandlers.

---

# Hosting Packages

One of the core design principles is the separation of business logic from hosting logic.

Event handlers should not depend on:

- ASP.NET Core
- Azure Functions
- HTTP abstractions
- Hosting-specific concerns

Instead, handlers depend only on:

```text
Entra.EventHandlers
```

or

```text
Entra.EventHandlers.Workforce
```

This allows the same handler implementation to run in different hosting environments.

---

## Entra.EventHandlers.AspNetCore

Provides ASP.NET Core hosting support.

Features:

- Router endpoints
- Single-event endpoints
- Dependency injection integration
- Request adaptation
- Response adaptation
- Event orchestration

Recommended when:

- Building Web APIs
- Hosting in ASP.NET Core applications
- Deploying containerized applications

---

## Entra.EventHandlers.AzureFunctions

Provides Azure Functions hosting support.

Features:

- Router functions
- Single-event functions
- Dependency injection integration
- Request adaptation
- Response adaptation
- Event orchestration

Recommended when:

- Building serverless applications
- Running on Azure Functions
- Scaling on demand

---

# Entra.EventHandlers.Security

The security package provides optional helpers for security-sensitive scenarios.

Current features include:

- PasswordSubmit payload decryption
- Azure Key Vault certificate integration
- RSA private key extraction
- Password context validation
- Certificate providers

Typical use cases:

- Password migration
- PasswordSubmit handlers
- Azure Key Vault integration
- Secure credential processing

Example:

```csharp
public class PasswordSubmitHandler(
    ILogger<PasswordSubmitHandler> logger,
    IPasswordContextDecryptor decryptor)
    : PasswordSubmitHandlerBase(logger)
{
}
```

This package is entirely optional.

Most applications do not require it.

---

# Request Processing Pipeline

Regardless of hosting model, incoming requests follow the same logical flow.

```text
Microsoft Entra Request
            │
            ▼
Hosting Adapter
            │
            ▼
Request Validation
            │
            ▼
Handler Resolution
            │
            ▼
Handler Execution
            │
            ▼
Response Builder
            │
            ▼
Microsoft Entra Response
```

This ensures consistent behavior across ASP.NET Core and Azure Functions.

---

# Dependency Injection

Handlers are resolved through the standard .NET dependency injection container.

Registration:

```csharp
builder.Services.AddEntraEventHandlers();
```

This automatically registers:

- Event orchestrator
- Handler resolver
- Request adapters
- Response adapters
- Event handlers discovered in the application

Benefits:

- Constructor injection
- Service reuse
- Automatic handler discovery
- Easy testing
- Familiar .NET development patterns

Example:

```csharp
public class AttributeCollectionStartHandler(
    ILogger<AttributeCollectionStartHandler> logger,
    ICustomerRepository repository)
    : AttributeCollectionStartHandlerBase(logger)
{
}
```

---

# Fluent Response Builders

Responses are created using fluent builders instead of manually constructing protocol objects.

Example:

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .SetPrefillValues()
        .Add("email", "user@contoso.com")
    .Done()
    .Build();
```

Benefits:

- Discoverable API surface
- Compile-time safety
- Reduced protocol knowledge requirements
- Consistent code patterns
- Easier maintenance

---

# Event-Centric Design

Each Microsoft Entra event is represented by:

- An event model
- A response model
- A handler base class
- Event-specific response builders

Example:

```text
TokenIssuanceStartEvent
TokenIssuanceStartResponse
TokenIssuanceStartHandlerBase
TokenIssuanceStartResponseBuilder
```

This design keeps implementation details close to the event they belong to.

---

# Testing Strategy

Because handlers are isolated from hosting infrastructure, they can be tested directly.

Example:

```csharp
var result = await handler.HandleAsync(request);
```

No ASP.NET Core host or Azure Functions runtime is required.

This enables:

- Fast unit tests
- Predictable behavior
- Easy mocking
- High test coverage
- Independent business logic validation

Recommended approach:

```text
Test handlers directly.
Keep hosts and functions thin.
```

---

# Choosing Packages

| Scenario | Package |
|-----------|----------|
| Event contracts only | Entra.EventHandlers.Abstractions |
| External ID handlers | Entra.EventHandlers |
| Workforce handlers | Entra.EventHandlers.Workforce |
| ASP.NET Core hosting | Entra.EventHandlers.AspNetCore |
| Azure Functions hosting | Entra.EventHandlers.AzureFunctions |
| PasswordSubmit decryption | Entra.EventHandlers.Security |

---

# Key Architectural Principles

The Entra.EventHandlers ecosystem is built around several core principles:

- Strong typing over raw JSON
- Event-centric programming model
- Separation of business and hosting concerns
- Consistent APIs across events
- Dependency injection by default
- Testability first
- Hosting model independence
- Minimal developer ceremony

These principles allow developers to focus on business logic rather than Microsoft Entra protocol details.

---

# Related Documentation

### Start Here

- ./getting-started.md

### Hosting

- ./hosting/aspnetcore.md
- ./hosting/azure-functions.md