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

The registration also enables:

- Automatic handler discovery
- Automatic handler resolution
- Event orchestration
- Request deserialization
- Response serialization

Most applications do not require additional Entra.EventHandlers service registrations.

---

## Router Function

The router function is the recommended approach for most applications.

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

For most applications, the router function provides the best developer experience and the lowest maintenance cost.

---

## Error Handling

The hosting adapter automatically handles expected and unexpected exceptions.

Examples include:

- Validation failures
- Unsupported event types
- Deserialization errors
- Unhandled application exceptions

Appropriate HTTP responses are generated automatically.

This removes repetitive exception-handling code from Azure Functions and keeps the hosting layer focused on request handling.

---

## Testing

Business logic should be implemented in handlers rather than Azure Functions.

Example:

```csharp
public sealed class EmailOtpSendHandler(
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

Because handlers are independent from Azure Functions hosting infrastructure, they can be tested directly.

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

- Starting Azure Functions
- Creating HTTP requests
- Running the Functions host

Benefits include:

- Fast unit tests
- Easier mocking
- Better separation of concerns
- Higher test coverage

Azure Functions should typically remain thin adapters while handlers contain the application behavior.

---

## Minimal Azure Functions Example

A minimal Azure Functions sample focused on a single event handler is available:

https://github.com/szubajak/entra-event-handlers-azurefunctions

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

Avoid placing business logic directly inside Azure Functions.

Handlers should contain application behavior while Functions remain transport adapters.

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

Focus testing efforts on handlers rather than Azure Functions.

This provides faster, simpler, and more reliable tests while keeping the hosting layer minimal.

---

## Related Documentation

### Start Here

- [Getting Started](../getting-started.md)
- [Architecture](../architecture.md)

### Events

- [AttributeCollectionStart](../events/attribute-collection-start.md)
- [AttributeCollectionSubmit](../events/attribute-collection-submit.md)
- [EmailOtpSet](../events/email-otp-send.md)
- [PasswordSubmit](../events/password-submit.md)
- [TokenIssuanceStart](../events/token-issuance-start.md)
- [VerifiedIdClaimValidation](../events/verified-id-claim-validation.md)

### Hosting

- [AspNetCore](./aspnetcore.md)

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