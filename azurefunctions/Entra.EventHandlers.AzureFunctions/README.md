# Entra.EventHandlers.AzureFunctions

Azure Functions hosting adapter for Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers.

This package provides the Azure Functions integration layer for the Entra.EventHandlers ecosystem, enabling production-ready event handlers with minimal boilerplate, dependency injection support, centralized routing, structured error handling, and full testability.

## Installation

```bash
dotnet add package Entra.EventHandlers.AzureFunctions
```

## Features

- Azure Functions Isolated Worker integration
- Multi-event router function support
- Single-event function base classes
- Automatic request deserialization
- Automatic response serialization
- Dynamic handler resolution
- Centralized event orchestration
- Structured error handling
- Dependency injection integration
- Fully testable architecture

## Recommended Hosting Model

The recommended approach is a single router function that can handle multiple Microsoft Entra event types.

Benefits:

- Single HTTP endpoint
- Centralized configuration
- Minimal boilerplate
- Automatic event dispatching
- Consistent error handling
- Easier maintenance

## Router Function Example

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
- Resolves the correct handler
- Executes the handler
- Serializes the response
- Maps errors to appropriate HTTP responses

## Dependency Injection

Register Entra.EventHandlers services:

```csharp
builder.Services.AddEntraEventHandlers();
```

This registers:

- Request adapters
- Response adapters
- Event orchestrator
- Handler resolver
- Event handlers

## Handler Resolution

Handlers are resolved dynamically using the event type provided by Microsoft Entra.

Example:

```csharp
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler<TEvent, TResponse>
        Resolve<TEvent, TResponse>()
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse;
}
```

This allows a single Azure Function endpoint to host multiple event handlers without custom routing logic.

## Optional Single-Event Functions

If desired, each event can be exposed through its own Azure Function.

Example:

```csharp
public sealed class TokenIssuanceStartFunction(
    ILogger<TokenIssuanceStartFunction> logger,
    ITokenIssuanceStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : TokenIssuanceStartFunctionBase(
        logger,
        handler,
        requestAdapter,
        responseAdapter)
{
    [Function("TokenIssuanceStart")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "post",
            Route = "tokenissuancestart")]
        HttpRequestData request)
            => InvokeAsync(request);
}
```

This model may be preferred when:

- Each event requires a dedicated endpoint
- Teams manage events independently
- Routing is handled externally

## Supported Event Types

The hosting adapter supports all event handlers implemented using the Entra.EventHandlers ecosystem, including:

### External ID

- AttributeCollectionStart
- AttributeCollectionSubmit
- EmailOtpSend
- PasswordSubmit
- TokenIssuanceStart

### Workforce

- VerifiedIdClaimValidation

## Testing

The hosting infrastructure is designed to be testable.

Because handlers remain isolated from hosting concerns, business logic can be tested without requiring a running Azure Functions host.

Benefits include:

- Fast unit tests
- Simple mocking
- Dependency injection support
- Clear separation of concerns

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Public contracts and protocol models |
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting |
| Entra.EventHandlers.Security | PasswordSubmit decryption and Azure Key Vault integration |

## Documentation

Full documentation, event guides, hosting guides, and samples:

https://github.com/szubajak/entra-eventhandlers/tree/main/docs

AI-friendly repository metadata:

https://github.com/szubajak/entra-eventhandlers/blob/main/llms.txt

## Samples

Azure Functions sample applications:

https://github.com/szubajak/entra-eventhandlers/tree/main/samples

A minimal standalone Azure Functions example focused on a single event handler is also available:

https://github.com/szubajak/entra-eventhandlers-azurefunctions

## License

This package is licensed under the Business Source License (BSL).

See the repository documentation for licensing details and commercial licensing information.

The Entra.EventHandlers.Abstractions package is licensed under MIT and may be used freely.

## Further Reading

Entra External ID .NET Handlers Deep Dive

https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437

Building CIAM-Ready Azure Functions with Entra.EventHandlers

https://medium.com/@jakub.szubarga/entra-eventhandlers-ciam-azure-functions-97c5e1940272