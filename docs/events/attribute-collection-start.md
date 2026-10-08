# Attribute Collection Start

The `AttributeCollectionStart` event is raised by Microsoft Entra External ID when an attribute collection step begins during a user journey.

This event allows your application to:

- Continue the flow using default Entra behavior
- Prefill attribute values before they are shown to the user
- Block the flow and display a custom error page

The event is typically used to retrieve user information from external systems and improve the sign-up or profile collection experience.

---

## When is this event called?

Microsoft Entra invokes the `AttributeCollectionStart` custom authentication extension before displaying the attribute collection form to the user.

Common use cases include:

- Prefilling email addresses from external systems
- Prefilling profile information
- Looking up customer records
- Validating whether registration should be allowed
- Blocking registrations from specific domains or organizations

---

## Handler Base Class

Implement `AttributeCollectionStartHandlerBase` and override `HandleCoreAsync`.

```csharp
public class AttributeCollectionStartHandler(ILogger<AttributeCollectionStartHandler> logger)
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

The base class automatically provides:

- Request validation
- Structured logging
- Correlation ID logging
- Event name and event type logging scopes
- Execution duration measurement
- Exception handling
- Automatic exception-to-ShowBlockPage mapping

---

## Request Model

The incoming request is represented by:

```csharp
AttributeCollectionStartEvent
```

The event contains information supplied by Microsoft Entra about the current user journey and authentication context.

Example:

```csharp
protected override Task<AttributeCollectionStartResponse> HandleCoreAsync(
    AttributeCollectionStartEvent request,
    CancellationToken cancellationToken = default)
{
    var correlationId = request.CorrelationId; 
 
    return Task.FromResult(
        EntraEventResponses
            .AttributeCollectionStart()
            .ContinueWithDefaultBehavior()
            .Build());
}
```

---

## Response Builder

Use:

```csharp
EntraEventResponses.AttributeCollectionStart()
```

## Available Responses

The `AttributeCollectionStart` event supports three response types.

The event supports the following actions:

| Action | Purpose |
|----------|----------|
| ContinueWithDefaultBehavior | Continue the flow |
| SetPrefillValues | Prefill form fields |
| ShowBlockPage | Stop the flow and display a message |

---

### ContinueWithDefaultBehavior

Allows Microsoft Entra to continue the attribute collection flow without modifications.

### When to use

Use this response when:

- No additional processing is required
- No attributes need to be prefilled
- The user is allowed to continue normally

### Example

```csharp
public class AttributeCollectionStartHandler(ILogger<AttributeCollectionStartHandler> logger)
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

### Result

Microsoft Entra displays the configured attribute collection form and continues the user journey.

---

### SetPrefillValues

Provides attribute values that are displayed as initial input when the attribute collection page is shown.

### When to use

Use this response when:

- Data already exists in an external system
- A user profile has been partially completed
- Information can be retrieved from a CRM
- You want to reduce manual data entry

### Example

Prefill a user's email address and display name.

```csharp
public class AttributeCollectionStartHandler(ILogger<AttributeCollectionStartHandler> logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    protected override Task<AttributeCollectionStartResponse> HandleCoreAsync(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken = default)
    {
        var attributes = new Dictionary<string, object>
        {
            ["email"] = "john.doe@contoso.com",
            ["displayName"] = "John Doe"
        };
 
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .SetPrefillValues(attributes)
                .Build());
    }
}
```

### Builder Syntax

For complex scenarios, the fluent builder can be used.

```csharp
return Task.FromResult(
    EntraEventResponses
        .AttributeCollectionStart()
        .SetPrefillValues()
            .Add("email", "john.doe@contoso.com")
            .Add("displayName", "John Doe")
        .Done()
        .Build());
```

### Result

The user sees the attribute collection form with pre-populated values.

Example:

```text
Email Address john.doe@contoso.com
Display Name John Doe
```

The user may review or modify those values depending on the Entra configuration.

---

### ShowBlockPage

Displays a custom block page and stops the current user journey.

### When to use

Use this response when:

- Registration is not allowed
- A user fails validation checks
- A required business rule is violated
- A user belongs to a blocked organization
- Disposable email domains are detected

### Example

Block all registrations from a specific email domain.

```csharp
public class AttributeCollectionStartHandler(ILogger<AttributeCollectionStartHandler> logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    protected override Task<AttributeCollectionStartResponse> HandleCoreAsync(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken = default)
    {
        const string blockedDomain = "example.net";
 
        bool registrationBlocked = true;
 
        if (registrationBlocked)
        {
            return Task.FromResult(
                EntraEventResponses
                    .AttributeCollectionStart()
                    .ShowBlockPage(
                        "Registration Not Allowed",
                        $"Registrations from {blockedDomain} are not permitted.")
                    .Build());
        }
 
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .ContinueWithDefaultBehavior()
                .Build());
    }
}
```

### Result

The user is shown a custom error page.

Example:

```text
Registration Not Allowed

Registrations from example.net are not permitted.
```

The attribute collection flow stops immediately.

---

# Real-World Example

The following example checks whether a customer record exists.

If a customer exists, profile information is prefilled.

If the customer is blocked, a block page is displayed.

Otherwise, the journey continues using the default behavior.

```csharp
public class AttributeCollectionStartHandler(ILogger<AttributeCollectionStartHandler> logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    protected override Task<AttributeCollectionStartResponse> HandleCoreAsync(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken = default)
    {
        var customer = GetCustomer();
 
        if (customer.IsBlocked)
        {
            return Task.FromResult(
                EntraEventResponses
                    .AttributeCollectionStart()
                    .ShowBlockPage(
                        "Account Blocked",
                        "Your account has been disabled. Please contact support.")
                    .Build());
        }
 
        if (customer.Exists)
        {
            return Task.FromResult(
                EntraEventResponses
                    .AttributeCollectionStart()
                    .SetPrefillValues()
                        .Add("email", customer.Email)
                        .Add("displayName", customer.DisplayName)
                    .Done()
                    .Build());
        }
 
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .ContinueWithDefaultBehavior()
                .Build());
        }
}
```

---

# Best Practices

## Keep Handlers Fast

Microsoft Entra invokes the handler during the user journey.

Avoid:

- Long-running database operations
- Slow external APIs
- Expensive computations

Prefer:

- Cached data
- Optimized lookups
- Simple validation logic

---

## Prefill Only Trusted Data

Only return attribute values that originate from trusted systems.

Example:

```csharp
.SetPrefillValues()
    .Add("email", customer.Email)
```

Avoid prefilling values from untrusted sources.

---

## Use Block Pages for Clear User Feedback

If a user cannot continue, return a meaningful message.

Good:

```csharp
.ShowBlockPage(
    "Registration Not Allowed",
    "Your organization is not eligible for self-service registration.")
```

Poor:

```csharp
.ShowBlockPage(
    "Error",
    "Something went wrong.")
```

---

## Return Exactly One Action

Every `AttributeCollectionStart` response returns a single action.

Choose one:

- `ContinueWithDefaultBehavior()`
- `SetPrefillValues(...)`
- `ShowBlockPage(...)`

---

# Summary

The Microsoft Entra External ID AttributeCollectionStart event is used to influence the beginning of an attribute collection step.

Supported responses:

| Response | Purpose |
|-----------|----------|
| ContinueWithDefaultBehavior | Continue the flow normally |
| SetPrefillValues | Provide initial attribute values |
| ShowBlockPage | Stop the flow and show a custom message |

This event is commonly used for customer lookups, profile enrichment, registration validation, and user experience improvements.