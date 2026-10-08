# Verified ID Claim Validation

The `VerifiedIdClaimValidation` event is a Microsoft Entra Workforce authentication event used during account recovery scenarios involving Microsoft Entra Verified ID credentials.

This event allows your application to:

- Validate claims presented by a Verified ID credential
- Verify that required claims are present
- Perform custom verification against business systems
- Reject invalid credentials
- Control whether account recovery can continue

The event is typically used to ensure that claims provided by a Verified ID credential satisfy organizational requirements before authentication proceeds.

---

## When is this event called?

Microsoft Entra invokes the `VerifiedIdClaimValidation` custom authentication extension during the account recovery flow when a user presents a Verified ID credential.

The event occurs after Microsoft Entra has validated the credential itself and extracted its claims.

Your application then decides whether those claims satisfy additional business requirements.

Common use cases include:

- Account recovery
- Employee verification
- Contractor verification
- Student verification
- Membership validation
- Business-specific claim checks

---

## Handler Base Class

Implement `VerifiedIdClaimValidationHandlerBase` and override `HandleCoreAsync`.

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
- Event name and event type logging scopes
- Execution duration measurement
- Exception handling
- Safe default behavior when exceptions occur

---

## Request Model

The incoming request is represented by:

```csharp
VerifiedIdClaimValidationEvent
```

The event contains:

```csharp
request.Data.VerifiedIdClaimsContext
```

which holds claims extracted from the Verified ID credential.

Example:

```csharp
protected override Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
    VerifiedIdClaimValidationEvent request,
    CancellationToken cancellationToken = default)
{
    var claimsContext = request.Data.VerifiedIdClaimsContext;

    return Task.FromResult(
        EntraWorkforceEventResponses
            .VerifiedIdClaimValidation()
            .Pass()
            .Build());
}
```

---

## Verified ID Claims Context

Claim data is available through:

```csharp
request.Data.VerifiedIdClaimsContext
```

The exact contents depend on:

- Credential type
- Credential issuer
- Microsoft Entra configuration
- Verified ID definition

Typical examples include:

- Employee identifiers
- Email addresses
- Organization identifiers
- Student identifiers
- Membership information

---

## Response Builder

Workforce events use:

```csharp
EntraWorkforceEventResponses
```

rather than:

```csharp
EntraEventResponses
```

To create a response:

```csharp
EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
```

---

## Available Responses

The event supports the following actions.

| Action | Purpose |
|----------|----------|
| Pass | Validation succeeds and recovery continues |
| Failed | Validation fails and one or more claims are rejected |

---

## Pass

Indicates that all required claims are valid.

### When to use

Use this response when:

- Required claims are present
- Claim values are valid
- Verification succeeds
- Account recovery should continue

### Example

```csharp
return Task.FromResult(
    EntraWorkforceEventResponses
        .VerifiedIdClaimValidation()
        .Pass()
        .Build());
```

### Result

Microsoft Entra continues the account recovery flow.

---

## Failed

Indicates that one or more claims failed validation.

### When to use

Use this response when:

- Required claims are missing
- Claim values are invalid
- Verification fails
- Account recovery should not continue

### Example

```csharp
return Task.FromResult(
    EntraWorkforceEventResponses
        .VerifiedIdClaimValidation()
        .Failed(new[]
        {
            "employeeId"
        })
        .Build());
```

### Builder Syntax

For more complex scenarios:

```csharp
return Task.FromResult(
    EntraWorkforceEventResponses
        .VerifiedIdClaimValidation()
        .Failed()
            .Add("employeeId")
            .Add("department")
        .Done()
        .Build());
```

### Result

Microsoft Entra treats the specified claims as invalid and recovery cannot proceed.

---

## Real-World Example

Validate that an employee identifier exists and belongs to an active employee.

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
        var claims = request.Data.VerifiedIdClaimsContext;

        var employeeId = claims?.Claims["employeeId"]?.ToString();

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

        if (!exists)
        {
            return EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Failed(new[]
                {
                    "employeeId"
                })
                .Build();
        }

        return EntraWorkforceEventResponses
            .VerifiedIdClaimValidation()
            .Pass()
            .Build();
    }
}
```

---

## Example: Organizational Validation

Allow account recovery only for users belonging to a particular organization.

```csharp
public class VerifiedIdClaimValidationHandler(
    ILogger<VerifiedIdClaimValidationHandler> logger)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
    protected override Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken = default)
    {
        var organization = request
            .Data
            .VerifiedIdClaimsContext?
            .Claims["organization"]?
            .ToString();

        if (organization != "Contoso")
        {
            return Task.FromResult(
                EntraWorkforceEventResponses
                    .VerifiedIdClaimValidation()
                    .Failed(new[]
                    {
                        "organization"
                    })
                    .Build());
        }

        return Task.FromResult(
            EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Pass()
                .Build());
    }
}
```

---

## Testing

Because validation logic lives inside handlers, it can be tested directly.

Example:

```csharp
var result = await handler.HandleAsync(request);
```

Test scenarios commonly include:

- Required claims present
- Missing claims
- Invalid claim values
- External verification failures
- Successful validation

without:

- Running Azure Functions
- Running ASP.NET Core
- Creating HTTP requests

---

## Best Practices

### Validate Only Required Claims

Focus on claims required for the recovery decision.

Avoid validating unrelated credential data.

---

### Keep Validation Fast

Avoid:

- Slow external systems
- Expensive queries
- Long-running workflows

Account recovery should remain responsive.

---

### Fail Explicitly

Good:

```csharp
.Failed(new[]
{
    "employeeId"
})
```

This clearly identifies which claim caused validation to fail.

---

### Use Dependency Injection

Inject validation services into handlers.

Example:

```csharp
public class VerifiedIdClaimValidationHandler(
    ILogger<VerifiedIdClaimValidationHandler> logger,
    IEmployeeDirectory employeeDirectory)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
}
```

---

### Return Exactly One Action

Every `VerifiedIdClaimValidation` response returns a single action.

Choose one:

- `Pass()`
- `Failed(...)`

---

## Summary

The Microsoft Entra Workforce `VerifiedIdClaimValidation` event allows applications to validate claims extracted from a Verified ID credential during account recovery.

Supported responses:

| Response | Purpose |
|-----------|----------|
| Pass | Validation succeeds |
| Failed | Validation fails for one or more claims |

This event is commonly used for account recovery, employee verification, membership validation, student verification, and organizational claim validation.