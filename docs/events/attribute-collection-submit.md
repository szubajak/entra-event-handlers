# Attribute Collection Submit

The `AttributeCollectionSubmit` event is raised by Microsoft Entra External ID after a user submits an attribute collection form.

This event allows your application to:

- Continue the flow using default Entra behavior
- Modify submitted attribute values
- Display field-level validation errors
- Block the flow and display a custom error page

The event is typically used to validate user input, enrich submitted data, enforce business rules, and normalize attribute values before they are persisted.

---

## When is this event called?

Microsoft Entra invokes the `AttributeCollectionSubmit` custom authentication extension after the user submits an attribute collection page.

The submitted values are included in the request and can be inspected, validated, modified, or rejected.

Common use cases include:

- Validating email addresses
- Validating company-specific registration rules
- Blocking disposable email domains
- Normalizing user input
- Modifying attribute values before persistence
- Enforcing business requirements
- Displaying validation errors for specific fields

---

## Handler Base Class

Implement `AttributeCollectionSubmitHandlerBase` and override `HandleCoreAsync`.

```csharp
public class AttributeCollectionSubmitHandler(
    ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ContinueWithDefaultBehavior()
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
- Default response (`ShowBlockPage`) when an unhandled exception occurs

---

## Request Model

The incoming request is represented by:

```csharp
AttributeCollectionSubmitEvent
```

The event contains the values submitted by the user during the attribute collection step.

Example:

```csharp
protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
    AttributeCollectionSubmitEvent request,
    CancellationToken cancellationToken = default)
{
    var correlationId = request.CorrelationId;

    return Task.FromResult(
        EntraEventResponses
            .AttributeCollectionSubmit()
            .ContinueWithDefaultBehavior()
            .Build());
}
```

---

## Response Builder

Use:

```csharp
EntraEventResponses.AttributeCollectionSubmit()
```

to create `AttributeCollectionSubmitResponse` instances.

---

## Available Responses

The event supports the following actions.

| Action | Purpose |
|----------|----------|
| ContinueWithDefaultBehavior | Continue the user journey |
| ModifyAttributeValues | Replace submitted values before persistence |
| ShowValidationError | Display field-level validation errors |
| ShowBlockPage | Stop the flow and display a custom message |

---

## ContinueWithDefaultBehavior

Allows Microsoft Entra to continue the flow without modifications.

### When to use

Use this response when:

- Submitted values are valid
- No data transformation is required
- The user is allowed to continue normally

### Example

```csharp
public class AttributeCollectionSubmitHandler(
    ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
```

### Result

Microsoft Entra continues the user journey using the submitted values.

---

## ModifyAttributeValues

Allows submitted attribute values to be modified before Microsoft Entra continues the flow.

### When to use

Use this response when:

- Normalizing user input
- Fixing formatting issues
- Enriching submitted attributes
- Applying business-specific transformations
- Standardizing data before persistence

### Example

Convert an email address to lowercase before storing it.

```csharp
public class AttributeCollectionSubmitHandler(
    ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        var attributes = new Dictionary<string, object>
        {
            ["email"] = "john.doe@contoso.com"
        };

        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ModifyAttributeValues(attributes)
                .Build());
    }
}
```

### Result

The modified values are used by Microsoft Entra instead of the original submitted values.

---

## ShowValidationError

Displays validation errors and returns the user to the attribute collection form.

### When to use

Use this response when:

- A submitted value is invalid
- Required business rules are not satisfied
- A field requires correction
- The user should be allowed to retry

### Example

Require users to register using a company email address.

```csharp
public class AttributeCollectionSubmitHandler(
    ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ShowValidationError(
                    "Please correct the highlighted fields.",
                    new Dictionary<string, string>
                    {
                        ["email"] = "Only company email addresses are allowed."
                    })
                .Build());
    }
}
```

### Result

The user remains on the attribute collection page and sees validation errors associated with specific fields.

Example:

```text
Please correct the highlighted fields.

Email:
Only company email addresses are allowed.
```

---

## ShowBlockPage

Displays a custom block page and terminates the flow.

### When to use

Use this response when:

- Registration is not permitted
- The user belongs to a blocked organization
- The flow must be terminated immediately
- Continuing would violate business policies

### Example

Block registrations from a prohibited partner organization.

```csharp
public class AttributeCollectionSubmitHandler(
    ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ShowBlockPage(
                    "Registration Not Allowed",
                    "Your organization is not eligible for registration.")
                .Build());
    }
}
```

### Result

The user sees a block page and the journey stops immediately.

Example:

```text
Registration Not Allowed

Your organization is not eligible for registration.
```

---

## Real-World Example

The following example validates submitted registration data.

If the email domain is blocked, the flow is terminated.

If the email is invalid, a validation error is displayed.

Otherwise, the email is normalized and the journey continues.

```csharp
public class AttributeCollectionSubmitHandler(
    ILogger<AttributeCollectionSubmitHandler> logger)
    : AttributeCollectionSubmitHandlerBase(logger)
{
    protected override Task<AttributeCollectionSubmitResponse> HandleCoreAsync(
        AttributeCollectionSubmitEvent request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Data.Attributes["email"]?.ToString();

        if (email?.EndsWith("@blocked.com", StringComparison.OrdinalIgnoreCase) == true)
        {
            return Task.FromResult(
                EntraEventResponses
                    .AttributeCollectionSubmit()
                    .ShowBlockPage(
                        "Registration Not Allowed",
                        "Registrations from this domain are not permitted.")
                    .Build());
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            return Task.FromResult(
                EntraEventResponses
                    .AttributeCollectionSubmit()
                    .ShowValidationError(
                        "Please correct the highlighted fields.",
                        new Dictionary<string, string>
                        {
                            ["email"] = "A valid email address is required."
                        })
                    .Build());
        }

        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionSubmit()
                .ModifyAttributeValues(
                    new Dictionary<string, object>
                    {
                        ["email"] = email.ToLowerInvariant()
                    })
                .Build());
    }
}
```

---

## Best Practices

### Keep Validation Fast

Microsoft Entra invokes the handler during the user journey.

Avoid:

- Long-running database operations
- Slow external APIs
- Expensive computations

Prefer:

- Cached lookups
- Lightweight validation
- Fast business-rule evaluation

---

### Prefer Validation Errors Over Block Pages

Use validation errors when the user can correct the problem.

Good:

```csharp
.ShowValidationError(...)
```

Use block pages only when the journey must be terminated.

Good:

```csharp
.ShowBlockPage(...)
```

---

### Normalize Data Before Persistence

Examples include:

- Lowercasing email addresses
- Standardizing phone formats
- Trimming whitespace
- Applying naming conventions

Use:

```csharp
.ModifyAttributeValues(...)
```

for these scenarios.

---

### Return Exactly One Action

Every `AttributeCollectionSubmit` response returns a single action.

Choose one:

- `ContinueWithDefaultBehavior()`
- `ModifyAttributeValues(...)`
- `ShowValidationError(...)`
- `ShowBlockPage(...)`

---

## Summary

The Microsoft Entra External ID `AttributeCollectionSubmit` event allows applications to validate, modify, or reject submitted attribute values before the user journey continues.

Supported responses:

| Response | Purpose |
|-----------|----------|
| ContinueWithDefaultBehavior | Continue the flow normally |
| ModifyAttributeValues | Replace submitted values |
| ShowValidationError | Return field-level validation errors |
| ShowBlockPage | Stop the flow and display a custom message |

This event is commonly used for registration validation, business-rule enforcement, data normalization, and profile enrichment.