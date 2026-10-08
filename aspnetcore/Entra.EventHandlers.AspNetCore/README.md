# Entra.EventHandlers.AspNetCore

ASP.NET Core hosting adapter for Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers.

This package provides ASP.NET Core integration for the Entra.EventHandlers ecosystem, enabling strongly typed event handlers with dependency injection, endpoint routing, request/response adapters, and centralized event orchestration.

## Installation

```bash
dotnet add package Entra.EventHandlers.AspNetCore
```

## Features

- ASP.NET Core integration
- Multi-event router endpoint
- Single-event endpoints
- Automatic request deserialization
- Automatic response serialization
- Dynamic handler resolution
- Centralized event orchestration
- Structured error handling
- Dependency injection integration
- Fully testable architecture

## Quick Start

Register Entra.EventHandlers:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraEventHandlers();

var app = builder.Build();

app.MapEntraRouter();

app.Run();
```

The router endpoint automatically:

- Deserializes requests
- Resolves handlers
- Executes handlers
- Serializes responses
- Maps exceptions to HTTP responses

---

## Recommended Hosting Model

The recommended approach is a single router endpoint that can handle multiple Microsoft Entra event types.

Benefits:

- Single HTTP endpoint
- Centralized configuration
- Minimal boilerplate
- Automatic event dispatching
- Consistent error handling
- Easier maintenance

---

## Router Endpoint

Register the router endpoint:

```csharp
app.MapEntraRouter();
```

The router provides:

- Automatic event deserialization
- Event orchestration
- Dynamic handler resolution
- Handler invocation
- Response serialization
- Structured exception handling

A single endpoint can process multiple event types using the same orchestration pipeline.

---

## Dependency Injection

Register Entra.EventHandlers services:

```csharp
builder.Services.AddEntraEventHandlers();
```

This automatically registers:

- Request adapters
- Response adapters
- Event orchestrator
- Handler resolver
- Event handlers discovered in the application
- ASP.NET Core endpoint implementations

Most applications do not require additional Entra.EventHandlers registrations.

---

## Handler Discovery

Handlers are automatically discovered and resolved using the incoming event type.

Example:

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
}
```

No manual registration is required.

When Microsoft Entra sends a matching event, the framework automatically resolves and executes the appropriate handler.

---

## Alternative: Single-Event Endpoints

The package also provides dedicated endpoint mappings for individual events.

Available mappings:

- `app.MapEntraAttributeCollectionStart()`
- `app.MapEntraAttributeCollectionSubmit()`
- `app.MapEntraTokenIssuanceStart()`
- `app.MapEntraEmailOtpSend()`
- `app.MapPasswordSubmit()`
- `app.MapVerifiedIdClaimValidation()`

Example:

```csharp
app.MapEntraTokenIssuanceStart();
```

Single-event endpoints may be useful when:

- Each event requires its own route
- Teams manage events independently
- Routing is handled externally

---

## Default Routes

| Endpoint | Route |
|----------|----------|
| Router | `/router` |
| AttributeCollectionStart | `/attributecollectionstart` |
| AttributeCollectionSubmit | `/attributecollectionsubmit` |
| TokenIssuanceStart | `/tokenissuancestart` |
| EmailOtpSend | `/emailotpsend` |
| PasswordSubmit | `/passwordsubmit` |
| VerifiedIdClaimValidation | `/verifiedidclaimvalidation` |

---

## Supported Event Types

The hosting adapter supports all event handlers implemented using the Entra.EventHandlers ecosystem.

### External ID

- AttributeCollectionStart
- AttributeCollectionSubmit
- EmailOtpSend
- PasswordSubmit
- TokenIssuanceStart

### Workforce

- VerifiedIdClaimValidation

---

## Testing

The hosting infrastructure is designed to be testable.

Because handlers remain isolated from hosting concerns, business logic can be tested without requiring ASP.NET Core hosting infrastructure.

Benefits include:

- Fast unit tests
- Simple mocking
- Dependency injection support
- Clear separation of concerns

Example:

```csharp
var result = await handler.HandleAsync(request);
```

The ASP.NET Core endpoints should remain thin adapters while handlers contain the application behavior.

---

## Documentation

Full documentation, event guides, hosting guides, and samples:

https://github.com/szubajak/entra-event-handlers/tree/main/docs

ASP.NET Core hosting guide:

https://github.com/szubajak/entra-event-handlers/blob/main/docs/hosting/aspnetcore.md

AI-friendly repository metadata:

https://github.com/szubajak/entra-event-handlers/blob/main/llms.txt

---

## Samples

ASP.NET Core sample application:

https://github.com/szubajak/entra-event-handlers/tree/main/samples/ApiSample

The sample demonstrates:

- Dependency injection setup
- Router endpoint hosting
- Single-event endpoint hosting
- Event orchestration
- Shared handler implementations

---

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Public contracts and protocol models |
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce