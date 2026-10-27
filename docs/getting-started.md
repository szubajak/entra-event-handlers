# Getting Started with Entra.EventHandlers

Entra.EventHandlers is a .NET ecosystem for building Microsoft Entra External ID and Microsoft Entra Workforce custom authentication extension event handlers.

It provides:

- Strongly typed event models
- Strongly typed response models
- Event orchestration
- Dependency injection integration
- Fluent response builders
- ASP.NET Core hosting integration
- Azure Functions hosting integration
- PasswordSubmit security integrations

This guide walks through the basic setup required to create and host an event handler.

---

## Installation

Install the implementation package:

```bash
dotnet add package Entra.EventHandlers
```

Install a hosting adapter for your preferred hosting model.

### ASP.NET Core

```bash
dotnet add package Entra.EventHandlers.AspNetCore
```

### Azure Functions

```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```

For PasswordSubmit migration scenarios, you may additionally need:

```bash
dotnet add package Entra.EventHandlers.Security
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
- Registers `IHttpAdapter`
- Enables handler resolution
- Configures the hosting pipeline

The same handler implementations can be hosted in either ASP.NET Core or Azure Functions without modification.

---

## Choose a Hosting Model

Entra.EventHandlers separates business logic from hosting concerns.

The same handler implementation can be hosted in different environments without code changes.

### ASP.NET Core

Use:

```bash
dotnet add package Entra.EventHandlers.AspNetCore
```

Features:

- Router endpoint
- Individual event endpoints
- Dependency injection integration
- IHttpAdapter integration
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
- IHttpAdapter integration
- Azure Functions Isolated Worker support

See:

- [Azure Functions Hosting](./hosting/azure-functions.md)
- [Azure Functions Sample](../samples/AzureFunctionsSample)

---

## Recommended Hosting Model

For most applications, the recommended approach is router-based hosting.

Benefits include:

- Single endpoint or function
- Minimal configuration
- Automatic event dispatching
- Consistent error handling
- Easier maintenance
- Support for all event types

Single-event endpoints and functions remain available when separate routes are required.

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

[Entra Event Handlers Sample](../samples/Sample.Common)

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
- [EmailOtpSend](./events/email-otp-send.md)
- [PasswordSubmit](./events/password-submit.md)
- [TokenIssuanceStart](./events/token-issuance-start.md)

### Workforce

- [VerifiedIdClaimValidation](./events/verified-id-claim-validation.md)

---

## Understanding the Ecosystem

The ecosystem is organized into layers:

```text
Entra.EventHandlers.Abstractions
        ↓
Entra.EventHandlers
Entra.EventHandlers.Workforce
        ↓
Entra.EventHandlers.AspNetCore
Entra.EventHandlers.AzureFunctions

Optional:

Entra.EventHandlers.Security
```

Typical External ID applications use:

```text
Entra.EventHandlers
+
Entra.EventHandlers.AspNetCore
```

or

```text
Entra.EventHandlers
+
Entra.EventHandlers.AzureFunctions
```

Workforce applications additionally use:

```text
Entra.EventHandlers.Workforce
```

PasswordSubmit migration scenarios may additionally use:

```text
Entra.EventHandlers.Security
```

---

## Next Steps

After completing this guide, continue with:

- [Architecture](./architecture.md)
- [Unit Testing](./testing.md)

### Hosting

- [AspNetCore](./hosting/aspnetcore.md)
- [AzureFunctions](./hosting/azure-functions.md)

Then explore the event-specific documentation for the event type you are implementing.