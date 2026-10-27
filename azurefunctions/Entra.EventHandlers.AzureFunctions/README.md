# Entra.EventHandlers.AzureFunctions

Azure Functions hosting adapter for Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers.

This package provides Azure Functions integration for the Entra.EventHandlers ecosystem, enabling strongly typed event handlers, dependency injection, event routing, HTTP adaptation, and centralized event orchestration.

## Installation

```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```

## Features

- Azure Functions Isolated Worker integration
- Multi-event router function
- Single-event function base classes
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
builder.Services.AddEntraEventHandlers();
```

Create a router function:

```csharp
public sealed class EntraEventRouterFunction(
    ILogger<EntraEventRouterFunction> logger,
    IHttpAdapter httpAdapter,
    IEntraEventOrchestrator orchestrator
) : EntraEventRouterFunctionBase(logger, httpAdapter, orchestrator)
{
    [Function("Router")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "post",
            Route = "router")]
        HttpRequestData req)
        => InvokeAsync(req);
}
```

The router function automatically:

- Deserializes incoming event requests
- Resolves handlers
- Executes handlers
- Serializes event responses
- Maps exceptions to HTTP responses
- Uses `IHttpAdapter` for HTTP request and response adaptation

---

## Recommended Hosting Model

The recommended approach is a single router function capable of handling multiple Microsoft Entra event types.

Benefits include:

- Single HTTP endpoint
- Centralized configuration
- Minimal boilerplate
- Automatic event dispatching
- Consistent error handling
- Easier maintenance

---

## Router Function

The router function provides:

- Automatic event deserialization
- Event orchestration
- Dynamic handler resolution
- Handler invocation
- Response serialization
- Structured exception handling

A single Azure Function can process multiple event types through the same orchestration pipeline.

---

## Dependency Injection

Register Entra.EventHandlers services:

```csharp
builder.Services.AddEntraEventHandlers();
```

This automatically registers:

- `IHttpAdapter`
- Event orchestrator
- Handler resolver
- Event handlers discovered in the application
- Azure Functions hosting components

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

## Alternative: Single-Event Functions

The package also provides dedicated function base classes for individual event types.

Available base classes:

- `AttributeCollectionStartFunctionBase`
- `AttributeCollectionSubmitFunctionBase`
- `EmailOtpSendFunctionBase`
- `PasswordSubmitFunctionBase`
- `TokenIssuanceStartFunctionBase`
- `VerifiedIdClaimValidationFunctionBase`

Example:

```csharp
public sealed class TokenIssuanceStartFunction(
    ILogger<TokenIssuanceStartFunction> logger,
    IHttpAdapter httpAdapter,
    ITokenIssuanceStartHandler handler
) : TokenIssuanceStartFunctionBase(
        logger,
        httpAdapter,
        handler)
{
    [Function("TokenIssuanceStart")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "post",
            Route = "tokenissuancestart")]
        HttpRequestData req)
        => InvokeAsync(req);
}
```

Single-event functions may be useful when:

- Each event requires its own endpoint
- Teams manage events independently
- Routing is handled externally

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

## Ecosystem Architecture

The Entra.EventHandlers ecosystem is organized into layers:

- Entra.EventHandlers.Abstractions provides shared protocol contracts
- Entra.EventHandlers provides External ID implementations
- Entra.EventHandlers.Workforce provides Workforce implementations
- Entra.EventHandlers.Security provides PasswordSubmit security integrations
- Entra.EventHandlers.AspNetCore provides ASP.NET Core hosting integration
- Entra.EventHandlers.AzureFunctions provides Azure Functions hosting integration

Applications typically reference one or more implementation packages together with a hosting package.

---

## Testing

The hosting infrastructure is designed to be testable.

Because handlers remain isolated from hosting concerns, business logic can be tested without requiring Azure Functions infrastructure.

Benefits include:

- Fast unit tests
- Simple mocking
- Dependency injection support
- Clear separation of concerns

Example:

```csharp
var result = await handler.HandleAsync(request);
```

Azure Functions classes should remain thin transport adapters while handlers contain the application behavior.

---

## Documentation

Full documentation, event guides, hosting guides, and samples:

https://github.com/szubajak/entra-event-handlers/tree/main/docs

Azure Functions hosting guide:

https://github.com/szubajak/entra-event-handlers/blob/main/docs/hosting/azure-functions.md

AI-friendly repository metadata:

https://github.com/szubajak/entra-event-handlers/blob/main/llms.txt

---

## Samples

Complete Azure Functions sample:

https://github.com/szubajak/entra-event-handlers/tree/main/samples/AzureFunctionsSample

Minimal Azure Functions sample:

https://github.com/szubajak/entra-event-handlers-azurefunctions

The samples demonstrate:

- Dependency injection setup
- Router function hosting
- Single-event function hosting
- Event orchestration
- Shared handler implementations

---

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Public contracts and protocol models |
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting adapter |
| Entra.EventHandlers.Security | PasswordSubmit decryption and Azure Key Vault integration |

## License

This package is licensed under the Business Source License (BSL).

The Entra.EventHandlers.Abstractions package is licensed under the MIT License.

See the repository for licensing details and commercial licensing information.

---

## Further Reading

**Entra External ID .NET Handlers Deep Dive**

https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437

**Building CIAM-Ready Azure Functions with Entra.EventHandlers**

https://medium.com/@jakub.szubarga/entra-eventhandlers-ciam-azure-functions-97c5e1940272