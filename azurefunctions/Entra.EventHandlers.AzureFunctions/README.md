# Entra.EventHandlers.AzureFunctions

Azure Functions hosting adapter for Microsoft Entra External ID and Microsoft Entra Workforce authentication event handlers.

This package provides Azure Functions integration for the Entra.EventHandlers ecosystem, enabling strongly typed event handlers with dependency injection, event routing, request/response adapters, and centralized orchestration.

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
- Dependency injection integration
- Structured error handling
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

- Deserializes requests
- Resolves handlers
- Executes handlers
- Serializes responses
- Maps exceptions to HTTP responses

## Alternative: Single-Event Functions

The package also provides dedicated function base classes:

- AttributeCollectionStartFunctionBase
- AttributeCollectionSubmitFunctionBase
- EmailOtpSendFunctionBase
- PasswordSubmitFunctionBase
- TokenIssuanceStartFunctionBase
- VerifiedIdClaimValidationFunctionBase

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
}
```

## Documentation

Full documentation:

https://github.com/szubajak/entra-event-handlers/tree/main/docs

Azure Functions hosting guide:

https://github.com/szubajak/entra-event-handlers/blob/main/docs/hosting/azure-functions.md

AI-friendly repository metadata:

https://github.com/szubajak/entra-event-handlers/blob/main/llms.txt

## Samples

Complete Azure Functions sample:

https://github.com/szubajak/entra-event-handlers/tree/main/samples/AzureFunctionsSample

Minimal Azure Functions sample:

https://github.com/szubajak/entra-event-handlers-azurefunctions

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Public contracts and protocol models |
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting |
| Entra.EventHandlers.Security | PasswordSubmit decryption and Azure Key Vault integration |

## License

This package is licensed under the Business Source License (BSL).

The Entra.EventHandlers.Abstractions package is licensed under MIT and may be used freely.

See the repository for licensing details and commercial licensing information.

## Further Reading

Entra External ID .NET Handlers Deep Dive

https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437

Building CIAM-Ready Azure Functions with Entra.EventHandlers

https://medium.com/@jakub.szubarga/entra-eventhandlers-ciam-azure-functions-97c5e1940272