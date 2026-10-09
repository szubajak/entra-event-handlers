# Getting Started with Entra.EventHandlers

Entra.EventHandlers is a .NET library for implementing Microsoft Entra External ID custom authentication extension event handlers.

It provides:

- Strongly typed event models
- Event handler dispatching
- Dependency injection integration
- Fluent response builders
- ASP.NET Core hosting integration
- Azure Functions hosting integration

This guide walks through the basic setup required to create and host an event handler.

---

## Installation

Install the core package:

```bash
dotnet add package Entra.EventHandlers
```

Install a hosting adapter for your preferred hosting model:

### ASP.NET Core

```bash
dotnet add package Entra.EventHandlers.AspNetCore
```

### Azure Functions

```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```

---

## Create an Event Handler

Implement a handler for the Microsoft Entra event you want to process.

The following example handles the Token Issuance Start event and adds custom claims to the issued token.

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        var userId = request.Data.AuthenticationContext?.User?.Id;

        string[] roles = userId switch
        {
            var id when id == Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                => ["Admin", "PowerUser"],

            _ => ["User"]
        };

        var customClaims = new Dictionary<string, object>
        {
            ["department"] = "Engineering",
            ["roles"] = roles
        };

        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken(customClaims)
                .Build());
    }
}
```

---

## Register Entra.EventHandlers

Register Entra.EventHandlers during application startup:

```csharp
builder.Services.AddEntraEventHandlers();
```

This single registration automatically:

- Discovers and registers event handlers
- Registers the event orchestrator
- Registers request and response adapters
- Enables handler resolution
- Configures the hosting pipeline

The same handler implementations can be used with either ASP.NET Core or Azure Functions.

---

## Choose a Hosting Model

Entra.EventHandlers separates business logic from hosting concerns.

The same handler can be hosted in different environments without modification.

### ASP.NET Core

Use:

```bash
dotnet add package Entra.EventHandlers.AspNetCore
```

Features:

- Router endpoint
- Individual event endpoints
- Dependency injection integration
- Minimal API support

See:

- [AspNetCore Hosting](./hosting/aspnetcore.md)
- [AspNetCore Sample](../samples/ApiSample)

---

### Azure Functions

Use:

```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```

Features:

- Router function
- Individual event functions
- Dependency injection integration
- Azure Functions Isolated Worker support

See:

- [Azure Functions Hosting](./hosting/azure-functions.md)
- [Azure Functions Sample](../samples/AzureFunctionsSample)

---

## Samples

Choose a sample based on what you want to learn.

### Sample.Common

Shared handler implementations demonstrating:

- Fluent response builders
- Custom claims
- Prefill values
- Block pages
- Event-specific business logic

[Entra Event Handlers Sample](samples/Sample.Common)

---

### ApiSample

Complete ASP.NET Core host using the shared handlers.

[AspNetCore Sample](../samples/ApiSample)

---

### AzureFunctionsSample

Complete Azure Functions host using the shared handlers.

[Azure Functions Sample](../samples/AzureFunctionsSample)

---

### Minimal Azure Functions Sample

https://github.com/szubajak/entra-event-handlers-azurefunctions

A minimal Azure Functions Isolated Worker example focused on:

- One handler
- One function
- Dependency injection
- Unit testing

Recommended when learning the Azure Functions hosting model for the first time.

---

## Event Documentation

Detailed documentation is available for each supported event.

### External ID

- [AttributeCollectionStart](./events/attribute-collection-start.md)
- [AttributeCollectionSubmit](./events/attribute-collection-submit.md)
- [AspNetCore](./events/email-otp-send.md)
- [PasswordSubmit](./events/password-submit.md)
- [TokenIssuanceStart](./events/token-issuance-start.md)

### Workforce

- [VerifiedIdClaimValidation](./events/verified-id-claim-validation.md)

---

## Next Steps

After completing this guide, continue with:

- [Architecture](./architecture.md)
- [Unit Testing](./testing.md)

### Hosting

- [AspNetCore](./hosting/aspnetcore.md)
- [AzureFunctions](./hosting/azure-functions.md)

Then explore the event-specific documentation for the event type you are implementing.