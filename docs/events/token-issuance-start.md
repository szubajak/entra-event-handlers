# Token Issuance Start

The `TokenIssuanceStart` event is raised by Microsoft Entra External ID immediately before a token is issued to an application.

This event allows your application to:

- Add custom claims to tokens
- Enrich tokens with data from external systems
- Include application-specific authorization data
- Include profile information in tokens
- Perform dynamic claims generation

The event is commonly used to augment ID tokens and access tokens with information that is not stored directly within Microsoft Entra.

---

## When is this event called?

Microsoft Entra invokes the `TokenIssuanceStart` custom authentication extension during token issuance.

The event occurs immediately before the token is generated and returned to the application.

Common use cases include:

- Adding application roles
- Adding custom permissions
- Including customer profile data
- Including subscription information
- Adding tenant-specific claims
- Providing data from external APIs
- Enriching authorization decisions

---

## Handler Base Class

Implement `TokenIssuanceStartHandlerBase` and override `HandleCoreAsync`.

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken(
                    new Dictionary<string, object>())
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
- Safe default behavior for unhandled exceptions

---

## Request Model

The incoming request is represented by:

```csharp
TokenIssuanceStartEvent
```

The event contains information about the current authentication operation and the user for whom the token is being generated.

Example:

```csharp
protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
    TokenIssuanceStartEvent request,
    CancellationToken cancellationToken = default)
{
    var correlationId = request.CorrelationId;

    return Task.FromResult(
        EntraEventResponses
            .TokenIssuanceStart()
            .ProvideClaimsForToken(
                new Dictionary<string, object>())
            .Build());
}
```

---

## Response Builder

Use:

```csharp
EntraEventResponses.TokenIssuanceStart()
```

to create `TokenIssuanceStartResponse` instances.

---

## Available Responses

The `TokenIssuanceStart` event supports a single response type.

| Action | Purpose |
|----------|----------|
| ProvideClaimsForToken | Adds custom claims to the issued token |

---

## ProvideClaimsForToken

Adds custom claims to the token being issued by Microsoft Entra.

### When to use

Use this response when:

- Adding custom roles
- Adding permissions
- Including profile information
- Including customer data
- Including tenant-specific attributes
- Including information from external systems

### Example

```csharp
return Task.FromResult(
    EntraEventResponses
        .TokenIssuanceStart()
        .ProvideClaimsForToken(
            new Dictionary<string, object>
            {
                ["department"] = "Engineering"
            })
        .Build());
```

### Result

The supplied claims become available in the issued token.

Example:

```json
{
  "department": "Engineering"
}
```

---

## Common Claim Types

Frequently added claims include:

```csharp
new Dictionary<string, object>
{
    ["department"] = "Engineering",
    ["subscription"] = "Premium",
    ["employeeId"] = "12345"
}
```

Role claims:

```csharp
new Dictionary<string, object>
{
    ["roles"] = new[]
    {
        "Admin",
        "PowerUser"
    }
}
```

Permission claims:

```csharp
new Dictionary<string, object>
{
    ["permissions"] = new[]
    {
        "Orders.Read",
        "Orders.Write"
    }
}
```

---

## Real-World Example

Determine token roles based on the authenticated user.

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        var userId = request.Data.AuthenticationContext?.User?.Id;

        string[] roles = userId switch
        {
            var id when id ==
                Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                    => ["Admin", "PowerUser"],

            _ => ["User"]
        };

        var customClaims = new Dictionary<string, object>
        {
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

Resulting token claims:

```json
{
  "department": "Engineering",
  "roles": [
    "Admin",
    "PowerUser"
  ]
}
```

---

## Example: Claims from External Systems

A common pattern is retrieving authorization data from an API or database.

```csharp
public class TokenIssuanceStartHandler(
    ILogger<TokenIssuanceStartHandler> logger,
    IUserProfileService profileService)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override async Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken = default)
    {
        var profile = await profileService.GetAsync(
            request.Data.AuthenticationContext.User.Id,
            cancellationToken);

        return EntraEventResponses
            .TokenIssuanceStart()
            .ProvideClaimsForToken(
                new Dictionary<string, object>
                {
                    ["customerTier"] = profile.Tier,
                    ["region"] = profile.Region
                })
            .Build();
    }
}
```

---

## Testing

Because token enrichment logic lives inside handlers, it can be tested without hosting infrastructure.

Example:

```csharp
var result = await handler.HandleAsync(request);
```

Typical assertions include:

```csharp
result.Response
    .Data
    .Actions
    .Single();
```

and validation of generated claims:

```csharp
claims["department"]
claims["roles"]
claims["permissions"]
```

without:

- Running Azure Functions
- Running ASP.NET Core
- Creating HTTP requests

---

## Best Practices

### Keep Token Enrichment Fast

Token issuance occurs during authentication.

Avoid:

- Slow external APIs
- Expensive database queries
- Long-running computations

Prefer:

- Cached data
- Optimized lookups
- Lightweight claim generation

---

### Add Only Required Claims

Include only the claims necessary for the consuming application.

Good:

```csharp
["department"] = "Engineering"
```

Avoid large payloads.

Large tokens can affect authentication performance and downstream applications.

---

### Use Stable Claim Names

Prefer:

```csharp
department
roles
employeeId
subscription
```

Avoid:

```csharp
engineeringDepartmentValue
customerPremiumLevelInternalV2
```

Stable claim names simplify client integration.

---

### Avoid Sensitive Data

Do not place highly sensitive information into tokens.

Examples include:

- Passwords
- API keys
- Secrets
- Certificate data

Tokens may be visible to applications and clients.

---

### Return Exactly One Action

Every `TokenIssuanceStart` response returns a single action.

Choose:

```csharp
.ProvideClaimsForToken(...)
```

No other response actions are supported.

---

## Summary

The Microsoft Entra External ID `TokenIssuanceStart` event allows applications to enrich issued tokens with custom claims.

Supported responses:

| Response | Purpose |
|-----------|----------|
| ProvideClaimsForToken | Add claims to the token being issued |

This event is commonly used for authorization data, application roles, permissions, profile enrichment, subscription information, and external system integration.