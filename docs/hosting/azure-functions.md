# Azure Functions Hosting

This guide explains how to host Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers using Azure Functions and the `Entra.EventHandlers.AzureFunctions` package.

The hosting adapter provides:

- Azure Functions Isolated Worker integration
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

Install the Azure Functions hosting adapter:

```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```

---

## Recommended Architecture

The recommended hosting model is a single router function that handles all Microsoft Entra event types.

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
Azure Function
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

Register Entra.EventHandlers services during startup.

```csharp
builder.Services.AddEntraEventHandlers();
```

This automatically registers:

- Event orchestrator
- Handler resolver
- Request adapters
- Response adapters
- Event handlers

---

## Router Function

The router function is the preferred approach for most applications.

```csharp
public sealed class EntraEventRouterFunction(
    ILogger<EntraEventRouterFunction> logger,
    IEntraEventOrchestrator orchestrator,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraEventRouterFunctionBase(
        logger,
        orchestrator,
        requestAdapter,
        responseAdapter)
{
    [Function("Router")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "post",
            Route = "router")]
        HttpRequestData request)
            => InvokeAsync(request);
}
```

The router automatically:

- Deserializes incoming requests
- Identifies the incoming event type
- Resolves the correct handler
- Executes the handler
- Converts the response to the Entra protocol format
- Maps exceptions to appropriate HTTP responses

---

## Handler Registration

Any handler registered with dependency injection becomes available to the router.

Example:

```csharp
builder.Services.AddScoped<
    IAttributeCollectionStartHandler,
    AttributeCollectionStartHandler>();
```

When Microsoft Entra sends an `AttributeCollectionStart` event, the router automatically resolves and executes the corresponding handler.

No custom routing logic is required.

---

## Available Function Base Classes

The package provides dedicated base classes for individual event types.

| Event | Function Base Class |
|---------|---------|
| AttributeCollectionStart | AttributeCollectionStartFunctionBase |
| AttributeCollectionSubmit | AttributeCollectionSubmitFunctionBase |
| EmailOtpSend | EmailOtpSendFunctionBase |
| PasswordSubmit | PasswordSubmitFunctionBase |
| TokenIssuanceStart | TokenIssuanceStartFunctionBase |
| VerifiedIdClaimValidation | VerifiedIdClaimValidationFunctionBase |

---

## Single-Event Functions

Although the router is recommended, individual event endpoints are also supported.

This approach creates one Azure Function per event type.

Example:

```csharp
public sealed class AttributeCollectionStartFunction(
    ILogger<AttributeCollectionStartFunction> logger,
    IAttributeCollectionStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : AttributeCollectionStartFunctionBase(
        logger,
        handler,
        requestAdapter,
        responseAdapter)
{
    [Function("AttributeCollectionStart")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "post",
            Route = "attributecollectionstart")]
        HttpRequestData request)
            => InvokeAsync(request);
}
```

The same pattern applies to all supported events.

---

## Router vs Single-Event Functions

| Scenario | Recommended Approach |
|-----------|-----------|
| New application | Router Function |
| Multiple event types | Router Function |
| Production workloads | Router Function |
| Proof-of-concept application | Router Function |
| Dedicated endpoint per event | Single-Event Functions |
| Separate ownership per event | Single-Event Functions |

For most applications, the router function provides the best developer experience and the smallest maintenance cost.

---

## Error Handling

The hosting adapter automatically handles expected and unexpected exceptions.

Examples include:

- Validation failures
- Unsupported event types
- Deserialization errors
- Unhandled application exceptions

Appropriate HTTP responses are generated automatically.

This avoids repetitive exception-handling code inside Azure Functions.

---

## Testing

Business logic should be implemented in handlers, not in Azure Functions.

Example:

```csharp
public class EmailOtpSendHandler
    : EmailOtpSendHandlerBase
{
}
```

This allows handlers to be tested without:

- Running Azure Functions
- Creating HTTP requests
- Starting a Functions host

Benefits:

- Fast unit tests
- Easier mocking
- Better separation of concerns

The Azure Function itself typically acts as a thin adapter.

---

## Minimal Azure Functions Example

A minimal Azure Functions sample focused on a single event handler is available:

https://github.com/szubajak/entra-eventhandlers-azurefunctions

The sample demonstrates:

- Dependency injection setup
- A single handler
- A single function
- Local testing
- Unit testing

This is the recommended starting point when learning the Azure Functions hosting model.

---

## Repository Samples

The main repository also contains complete Azure Functions examples.

Available under:

```text
samples/
├── AzureFunctionsSample
└── Sample.Common
```

These samples demonstrate:

- Router functions
- Single-event functions
- Shared handler implementations
- Dependency injection
- Event orchestration

---

## Best Practices

### Prefer the Router Function

Use a single router endpoint whenever possible.

Benefits:

- Less code
- Fewer functions
- Centralized configuration
- Simpler deployment

---

### Keep Business Logic in Handlers

Good:

```csharp
EmailOtpSendHandler
TokenIssuanceStartHandler
AttributeCollectionStartHandler
```

Avoid placing business logic directly inside Azure Functions.

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

Most testing effort should focus on handlers.

Azure Functions should remain thin adapters between Microsoft Entra and your application code.

---

## Related Documentation

- ../getting-started.md
- ../architecture.md
- ../events/attribute-collection-start.md
- ../events/attribute-collection-submit.md
- ../events/email-otp-send.md
- ../events/password-submit.md
- ../events/token-issuance-start.md
- ../events/verified-id-claim-validation.md

---

## Summary

The Azure Functions hosting adapter enables Microsoft Entra authentication events to run in Azure Functions using a strongly typed, dependency injection-friendly programming model.

Recommended approach:

1. Register services using `AddEntraEventHandlers()`
2. Use `EntraEventRouterFunctionBase`
3. Implement event handlers
4. Keep business logic inside handlers
5. Test handlers independently

For most applications, the router function provides the simplest and most maintainable hosting model.