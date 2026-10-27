# Entra.EventHandlers.Workforce

Implementation layer for building Microsoft Entra Workforce authentication event handlers in .NET.

This package builds on top of the MIT-licensed `Entra.EventHandlers.Abstractions` package and provides Workforce-specific event models, fluent response builders, handler base classes, validation, logging, exception handling, and execution infrastructure for Microsoft Entra Workforce authentication extensions.

This package currently focuses on the Workforce account recovery event:

- VerifiedIdClaimValidation

The package is designed to work alongside the ASP.NET Core and Azure Functions hosting adapters.

Unlike `Entra.EventHandlers`, which focuses on External ID events, this package provides Workforce-specific functionality.

## Installation

```bash
dotnet add package Entra.EventHandlers.Workforce
```

## Features

- Workforce event models
- Workforce response models
- Fluent response builders
- Workforce handler base classes
- Structured logging
- Correlation ID logging
- Protocol validation
- Exception handling
- Execution timing
- Dependency injection integration
- Fully testable architecture

## Supported Workforce Events

### VerifiedIdClaimValidation

The event is used during Microsoft Entra Workforce account recovery flows involving Verified ID credentials.

Supported responses:

- Pass
- Failed

Common scenarios include:

- Account recovery
- Employee verification
- Membership validation
- Student verification
- Organizational claim validation

---

## Unified Workforce Response Builder API

All Workforce response builders are available through a single entry point:

```csharp
EntraWorkforceEventResponses
    .VerifiedIdClaimValidation();
```

This provides a consistent and discoverable API across Workforce event handlers.

---

## Building Responses

Successful validation:

```csharp
return EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
    .Pass()
    .Build();
```

Validation failure:

```csharp
return EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
    .Failed(new[]
    {
        "employeeId",
        "department"
    })
    .Build();
```

Builder syntax:

```csharp
return EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
    .Failed()
        .Add("employeeId")
        .Add("department")
    .Done()
    .Build();
```

The fluent builders help ensure protocol-correct responses without requiring manual JSON construction.

---

## Implementing a Handler

Create a handler by inheriting from the Workforce-specific handler base class.

Example:

```csharp
public class VerifiedIdClaimValidationHandler(
    ILogger<VerifiedIdClaimValidationHandler> logger)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
    protected override Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Pass()
                .Build());
    }
}
```

The base classes automatically provide:

- Request validation
- Structured logging
- Correlation ID logging
- Event type logging
- Execution timing
- Exception handling

This allows handlers to focus entirely on business logic.

---

## Real-World Example

Validate claims supplied by a Verified ID credential against an internal directory.

```csharp
public class VerifiedIdClaimValidationHandler(
    ILogger<VerifiedIdClaimValidationHandler> logger,
    IEmployeeDirectory employeeDirectory)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
    protected override async Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken = default)
    {
        var employeeId = request
            .Data
            .VerifiedIdClaimsContext?
            .Claims["employeeId"]?
            .ToString();

        if (string.IsNullOrWhiteSpace(employeeId))
        {
            return EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Failed(new[]
                {
                    "employeeId"
                })
                .Build();
        }

        var exists = await employeeDirectory.ExistsAsync(
            employeeId,
            cancellationToken);

        return exists
            ? EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Pass()
                .Build()
            : EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Failed(new[]
                {
                    "employeeId"
                })
                .Build();
    }
}
```

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

## Ecosystem Architecture

The Entra.EventHandlers ecosystem is organized into layers:

- Entra.EventHandlers.Abstractions provides shared protocol contracts
- Entra.EventHandlers provides External ID implementations
- Entra.EventHandlers.Workforce provides Workforce implementations
- Entra.EventHandlers.Security provides PasswordSubmit security integrations
- Entra.EventHandlers.AspNetCore and Entra.EventHandlers.AzureFunctions provide hosting integrations

Most applications reference this package together with one of the hosting packages.

---

## Samples

Shared handler implementations are available in:

https://github.com/szubajak/entra-event-handlers/tree/main/samples/Sample.Common

The sample demonstrates:

- Workforce handler implementations
- Fluent Workforce response builders
- Verification patterns
- Dependency injection
- Production-ready handler structure

These handlers can be hosted using either ASP.NET Core or Azure Functions.

---

## Documentation

Full documentation, event guides, hosting guides, testing guidance, security guidance, and samples:

https://github.com/szubajak/entra-event-handlers/tree/main/docs

Verified ID claim validation documentation:

https://github.com/szubajak/entra-event-handlers/blob/main/docs/events/verified-id-claim-validation.md

AI-friendly repository metadata:

https://github.com/szubajak/entra-event-handlers/blob/main/llms.txt

---

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Public contracts and protocol models |
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting adapter |
| Entra.EventHandlers.AzureFunctions | Azure Functions hosting adapter |
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