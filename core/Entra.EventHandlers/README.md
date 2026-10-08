# Entra.EventHandlers

Production-ready implementation layer for Microsoft Entra External ID authentication event handlers.

This package builds on top of the MIT-licensed `Entra.EventHandlers.Abstractions` package and provides fluent response builders, handler base classes, validation, logging, and other developer-focused capabilities for building Microsoft Entra External ID custom authentication extensions.

This package focuses exclusively on External ID events.

Microsoft Entra Workforce events are provided separately by `Entra.EventHandlers.Workforce`.

## Installation

```bash
dotnet add package Entra.EventHandlers
```

## Features

- Fluent response builders
- Strongly typed event models
- Strongly typed response models
- Event handler base classes
- Structured logging
- Correlation ID logging
- Protocol validation
- Exception handling
- Execution timing
- Dependency injection integration
- Fully testable architecture

## Supported External ID Events

- AttributeCollectionStart
- AttributeCollectionSubmit
- EmailOtpSend
- PasswordSubmit
- TokenIssuanceStart

Each event includes:

- Event models
- Response models
- Handler base classes
- Fluent response builders
- Validation support

## Unified Response Builder API

All External ID response builders are available through a single entry point:

```csharp
EntraEventResponses.AttributeCollectionStart();

EntraEventResponses.AttributeCollectionSubmit();

EntraEventResponses.EmailOtpSend();

EntraEventResponses.PasswordSubmit();

EntraEventResponses.TokenIssuanceStart();
```

This provides a consistent and discoverable experience across all supported events.

---

## Building Responses

Example:

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .SetPrefillValues()
        .Add("email", "user@example.com")
        .Add("country", "PL")
    .Done()
    .Build();
```

The fluent builders ensure protocol-correct responses without requiring manual JSON construction.

---

## Implementing a Handler

Create a handler by inheriting from the appropriate event-specific base class.

Example:

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        var userId =
            request.Data.AuthenticationContext?.User?.Id;

        var roles = userId switch
        {
            var id when id ==
                Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                    => ["Admin", "PowerUser"],

            _ => ["User"]
        };

        var customClaims = new Dictionary<string, object>
        {
            ["tenantId"] = "contoso-eu",
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

The base class automatically provides:

- Request validation
- Structured logging
- Correlation ID logging
- Event type logging
- Execution timing
- Exception handling

This allows handlers to focus entirely on business logic.

---

## PasswordSubmit Support

The package includes the PasswordSubmit handler infrastructure.

Example:

```csharp
public class PasswordSubmitHandler(
    ILogger<PasswordSubmitHandler> logger,
    IPasswordContextDecryptor decryptor)
    : PasswordSubmitHandlerBase(logger, decryptor)
{
    protected override Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .PasswordSubmit()
                .WithNonce(decrypted.Nonce)
                .MigratePassword()
                .Build());
    }
}
```

For Azure Key Vault-based password decryption support, see:

- Entra.EventHandlers.Security

---

## Testing

Handlers are designed to be tested directly without ASP.NET Core or Azure Functions hosting infrastructure.

Example:

```csharp
var result = await handler.HandleAsync(request);
```

Benefits include:

- Fast unit tests
- Easier mocking
- Better separation of concerns
- Higher test coverage

Business logic should live inside handlers while hosting adapters remain thin transport layers.

---

## Samples

Shared handler implementations are available in:

https://github.com/szubajak/entra-event-handlers/tree/main/samples/Sample.Common

The sample demonstrates:

- Handler base classes
- Fluent response builders
- Custom claims
- Prefill values
- Validation responses
- Block pages
- Production-ready handler patterns

These handlers are reused by both the ASP.NET Core and Azure Functions sample applications.

---

## Documentation

Full documentation, event guides, hosting guides, testing guidance, security guidance, and samples:

https://github.com/szubajak/entra-event-handlers/tree/main/docs

AI-friendly repository metadata:

https://github.com/szubajak/entra-event-handlers/blob/main/llms.txt

---

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Public contracts and protocol models |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting |
| Entra.EventHandlers.AzureFunctions | Azure Functions hosting |
| Entra.EventHandlers.Security | PasswordSubmit decryption and Azure Key Vault integration |

## License

This package is licensed under the Business Source License (BSL).

The Entra.EventHandlers.Abstractions package is licensed under MIT and may be used freely.

See the repository for licensing details and commercial licensing information.

---

## Further Reading

Entra External ID .NET Handlers Deep Dive

https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437

Building CIAM-Ready Azure Functions with Entra.EventHandlers

https://medium.com/@jakub.szubarga/entra-eventhandlers-ciam-azure-functions-97c5e1940272