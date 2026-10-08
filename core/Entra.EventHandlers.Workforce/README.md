# Entra.EventHandlers.Workforce

Production-ready Workforce implementation layer for Microsoft Entra Workforce account recovery authentication events.

This package builds on top of the MIT-licensed `Entra.EventHandlers.Abstractions` package and provides strongly typed Workforce event models, fluent response builders, and handler base classes for Microsoft Entra Workforce authentication extensions.

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

The event is used during Workforce account recovery flows involving Verified ID credentials.

Supported responses:

- Pass
- Failed

Common scenarios:

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

This provides a consistent and discoverable API for Workforce event handlers.

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

The fluent builders ensure protocol-correct responses without requiring manual JSON construction.

---

## Implementing a Handler

Create a handler by inheriting from the Workforce handler base class.

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

The base class automatically provides:

- Request validation
- Structured logging
- Correlation ID logging
- Event type logging
- Execution timing
- Exception handling

This allows handlers to focus entirely on business logic.

---

## Real-World Example

Validate claims supplied by a Verified ID credential.

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

| Package | 