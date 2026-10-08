# ASP.NET Core Hosting

This guide explains how to host Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers using ASP.NET Core and the `Entra.EventHandlers.AspNetCore` package.

The hosting adapter provides:

- ASP.NET Core integration
- Strongly typed event handling
- Dependency injection integration
- Automatic request deserialization
- Automatic response serialization
- Dynamic handler resolution
- Structured error handling
- Centralized event orchestration

---

## Installation

Install the core package:

```bash
dotnet add package Entra.EventHandlers
```

Install the ASP.NET Core hosting adapter:

```bash
dotnet add package Entra.EventHandlers.AspNetCore
```

---

## Recommended Architecture

The recommended hosting model is a single router endpoint that handles all Microsoft Entra event types.

Benefits include:

- A single endpoint for all events
- Minimal boilerplate
- Centralized routing
- Consistent error handling
- Easier maintenance
- Simpler deployment

The overall request flow is:

```text
Microsoft Entra
        │
        ▼
ASP.NET Core Endpoint
        │
        ▼
Request Adapter
        │
        ▼
Event Orchestrator
        │
        ▼
Handler Resolution
        │
        ▼
Event Handler
        │
        ▼
Response Adapter
        │
        ▼
Microsoft Entra
```

---

## Dependency Injection

Register Entra.EventHandlers during application startup:

```csharp
builder.Services.AddEntraEventHandlers();
```

This automatically registers:

- Event orchestrator
- Handler resolver
- Request adapters
- Response adapters
- Event handlers discovered in the application
- ASP.NET Core endpoint implementations

The registration also enables:

- Automatic handler discovery
- Automatic handler resolution
- Event orchestration
- Request deserialization
- Response serialization

Most applications do not require additional Entra.EventHandlers service registrations.

---

## Router Endpoint

The router endpoint is the recommended approach for most applications.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraEventHandlers();

var app = builder.Build();

app.MapEntraRouter();

app.Run();
```

The router automatically:

- Deserializes incoming requests
- Identifies the incoming event type
- Resolves the correct handler
- Executes the handler
- Serializes the response
- Maps exceptions to appropriate HTTP responses

No custom routing logic is required.

---

## Handler Discovery

Event handlers are automatically discovered and registered when calling:

```csharp
builder.Services.AddEntraEventHandlers();
```

Example:

```csharp
public class AttributeCollectionStartHandler(
    ILogger<AttributeCollectionStartHandler> logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    protected override Task<AttributeCollectionStartResponse> HandleCoreAsync(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
```

No additional registration is required.

When Microsoft Entra sends an `AttributeCollectionStart` event, the router automatically resolves and executes the corresponding handler.

The same applies to all supported event types.

---

## Available Endpoint Mappings

The package provides endpoint mappings for all supported events.

| Event | Endpoint Mapping |
|---------|---------|
| Router | `MapEntraRouter()` |
| AttributeCollectionStart | `MapEntraAttributeCollectionStart()` |
| AttributeCollectionSubmit | `MapEntraAttributeCollectionSubmit()` |
| EmailOtpSend | `MapEntraEmailOtpSend()` |
| PasswordSubmit | `MapPasswordSubmit()` |
| TokenIssuanceStart | `MapEntraTokenIssuanceStart()` |
| VerifiedIdClaimValidation | `MapVerifiedIdClaimValidation()` |

---

## Single-Event Endpoints

Although the router is recommended, individual event endpoints are also supported.

Example:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraEventHandlers();

var app = builder.Build();

app.MapEntraTokenIssuanceStart();

app.Run();
```

The same pattern applies to all supported events.

---

## Router vs Single-Event Endpoints

| Scenario | Recommended Approach |
|-----------|-----------|
| New application | Router Endpoint |
| Multiple event types | Router Endpoint |
| Production workloads | Router Endpoint |
| Proof-of-concept application | Router Endpoint |
| Dedicated endpoint per event | Single-Event Endpoint |
| Separate ownership per event | Single-Event Endpoint |

For most applications, the router endpoint provides the best developer experience and the lowest maintenance cost.

---

## Default Routes

The default endpoint routes are:

| Endpoint | Route |
|----------|----------|
| Router | `/router` |
| AttributeCollectionStart | `/attributecollectionstart` |
| AttributeCollectionSubmit | `/attributecollectionsubmit` |
| EmailOtpSend | `/emailotpsend` |
| PasswordSubmit | `/passwordsubmit` |
| TokenIssuanceStart | `/tokenissuancestart` |
| VerifiedIdClaimValidation | `/verifiedidclaimvalidation` |

---

## Error Handling

The hosting adapter automatically handles expected and unexpected exceptions.

Examples include:

- Validation failures
- Unsupported event types
- Deserialization errors
- Unhandled application exceptions

Appropriate HTTP responses are generated automatically.

This removes repetitive exception-handling code from ASP.NET Core endpoints and keeps the hosting layer focused on request handling.

---

## Testing

Business logic should be implemented in handlers rather than ASP.NET Core endpoints.

Example:

```csharp
public class EmailOtpSendHandler(
    ILogger<EmailOtpSendHandler> logger,
    IEmailSender emailSender)
    : EmailOtpSendHandlerBase(logger)
{
    protected override async Task<EmailOtpSendResponse> HandleCoreAsync(
        EmailOtpSendEvent request,
        CancellationToken cancellationToken = default)
    {
        var otpContext = request.Data.OtpContext;

        await emailSender.SendOtpAsync(
            otpContext.Identifier,
            otpContext.OneTimeCode,
            cancellationToken);

        return EntraEventResponses.EmailOtpSend()
            .ContinueWithDefaultBehavior()
            .Build();
    }
}
```

Because handlers are independent from ASP.NET Core hosting infrastructure, they can be tested directly.

Example:

```csharp
[Fact]
public async Task HandleAsync_SendsOtp_AndContinuesDefaultBehavior()
{
    var result = await handler.HandleAsync(request);

    await emailSender
        .Received(1)
        .SendOtpAsync(...);

    result.HasException.Should().BeFalse();
}
```

This approach allows testing:

- Business logic
- External service interactions
- Response generation
- Error paths

without:

- Starting ASP.NET Core
- Creating HTTP requests
- Running a web server

Benefits include:

- Fast unit tests
- Easier mocking
- Better separation of concerns
- Higher test coverage

ASP.NET Core endpoints should typically remain thin adapters while handlers contain the application behavior.

---

## Repository Samples

The main repository contains complete ASP.NET Core examples.

Available under:

```text
samples/
├── ApiSample
└── Sample.Common
```

These samples demonstrate:

- Router endpoints
- Single-event endpoints
- Shared handler implementations
- Dependency injection
- Event orchestration

---

## Best Practices

### Prefer the Router Endpoint

Use a single router endpoint whenever possible.

Benefits:

- Less code
- Fewer endpoint definitions
- Centralized configuration
- Consistent behavior
- Simpler deployment

---

### Keep Business Logic in Handlers

Good:

```csharp
EmailOtpSendHandler
TokenIssuanceStartHandler
AttributeCollectionStartHandler
```

Avoid placing business logic directly inside ASP.NET Core endpoints.

Handlers should contain application behavior while endpoints remain transport adapters.

---

### Use Dependency Injection

Inject application services into handlers.

Example:

```csharp
public class EmailOtpSendHandler(
    ILogger<EmailOtpSendHandler> logger,
    IEmailSender emailSender)
    : EmailOtpSendHandlerBase(logger)
{
}
```

---

### Test Handlers Directly

Focus testing efforts on handlers rather than ASP.NET Core endpoints.

This provides faster, simpler, and more reliable tests while keeping the hosting layer minimal.

---

## Related Documentation

### Start Here

- ../getting-started.md
- ../architecture.md

### Events

- ../events/attribute-collection-start.md
- ../events/attribute-collection-submit.md
- ../events/email-otp-send.md
- ../events/password-submit.md
- ../events/token-issuance-start.md
- ../events/verified-id-claim-validation.md

### Hosting

- ./azure-functions.md

---

## Summary

The ASP.NET Core hosting adapter enables Microsoft Entra authentication events to run in ASP.NET Core using a strongly typed, dependency injection-friendly programming model.

Recommended approach:

1. Register services using `AddEntraEventHandlers()`
2. Use `app.MapEntraRouter()`
3. Implement event handlers
4. Keep business logic inside handlers
5. Test handlers independently

For most applications, the router endpoint provides the simplest and most maintainable hosting model.