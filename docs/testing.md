# Testing

One of the primary design goals of Entra.EventHandlers is testability.

Business logic is implemented inside handlers while ASP.NET Core and Azure Functions remain thin transport adapters.

This allows handlers to be tested directly without running:

- ASP.NET Core
- Azure Functions
- HTTP servers
- Hosting infrastructure

---

## Testing Philosophy

The recommended approach is:

```text
Business Logic
        ↓
Handler
        ↓
Unit Test
```

rather than:

```text
Business Logic
        ↓
ASP.NET Core
        ↓
HTTP Request
        ↓
Integration Test
```

or:

```text
Business Logic
        ↓
Azure Functions Host
        ↓
Function Runtime
        ↓
Integration Test
```

Most application behavior should be tested at the handler level.

---

## Why Test Handlers Directly?

Benefits include:

- Faster execution
- Easier debugging
- Simpler mocking
- No web server required
- No Azure Functions runtime required
- Better isolation
- Higher test coverage

Handlers are ordinary C# classes and can be tested like any other application service.

---

## Typical Test Structure

A handler usually depends on:

- Logger
- Application services
- Repositories
- External APIs

Example:

```csharp
public sealed class EmailOtpSendHandler(
    ILogger<EmailOtpSendHandler> logger,
    IEmailSender emailSender)
    : EmailOtpSendHandlerBase(logger)
{
}
```

These dependencies can be mocked using your preferred mocking framework.

---

## Example Test

The following example tests an EmailOtpSend handler.

Handler:

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
        await emailSender.SendOtpAsync(
            request.Data.OtpContext.Identifier,
            request.Data.OtpContext.OneTimeCode,
            cancellationToken);

        return EntraEventResponses
            .EmailOtpSend()
            .ContinueWithDefaultBehavior()
            .Build();
    }
}
```

Test:

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

This verifies:

- Business logic execution
- Service interactions
- Generated response
- Successful completion

without any hosting infrastructure.

---

## Recommended Test Categories

### Happy Path

Verify the expected business behavior.

Example:

```csharp
[Fact]
public async Task HandleAsync_ReturnsExpectedResponse()
{
}
```

---

### Dependency Interaction

Verify that external services are called correctly.

Example:

```csharp
await emailSender
    .Received(1)
    .SendOtpAsync(...);
```

---

### Error Handling

Verify behavior when dependencies fail.

Example:

```csharp
_emailSender
    .SendOtpAsync(...)
    .Throws(new InvalidOperationException());
```

and:

```csharp
result.HasException.Should().BeTrue();
```

---

### Response Validation

Verify the generated Entra response.

Example:

```csharp
response.Data.Actions
    .Should()
    .ContainSingle();
```

---

## Testing EmailOtpSend

Recommended assertions:

- Email provider invoked
- OTP value forwarded correctly
- Correct response action returned
- Exceptions handled correctly

---

## Testing AttributeCollectionStart

Recommended assertions:

- Prefill values generated correctly
- Block page generated correctly
- Correct action returned

Example:

```csharp
.SetPrefillValues(...)
```

or

```csharp
.ShowBlockPage(...)
```

---

## Testing AttributeCollectionSubmit

Recommended assertions:

- Validation errors generated correctly
- Modified attributes generated correctly
- Blocking behavior works correctly

Example:

```csharp
.ShowValidationError(...)
```

or

```csharp
.ModifyAttributeValues(...)
```

---

## Testing TokenIssuanceStart

Recommended assertions:

- Custom claims generated correctly
- Roles generated correctly
- Claim values match application expectations

Example:

```csharp
.ProvideClaimsForToken(...)
```

---

## Testing PasswordSubmit

Recommended assertions:

- Password validation behavior
- Nonce preservation
- Migration responses
- Retry responses
- Block responses

Mock:

```csharp
IPasswordContextDecryptor
```

to avoid real cryptography and certificate dependencies during unit tests.

---

## Testing VerifiedIdClaimValidation

Recommended assertions:

- Required claims validated
- Failed claims identified correctly
- Pass responses generated when validation succeeds

Example:

```csharp
.Pass()
```

or

```csharp
.Failed(...)
```

---

## What Not to Test

Avoid unit testing:

- ASP.NET Core endpoint mappings
- Azure Function trigger attributes
- Request adapters
- Response adapters
- Framework plumbing

These components already contain minimal logic.

Focus tests on:

```text
Handler
        +
Business Logic
```

---

## Integration Testing

Use integration tests only when validating:

- Dependency injection configuration
- ASP.NET Core routing
- Azure Functions hosting
- Database interactions
- External APIs

Most business logic should remain covered by handler-level unit tests.

---

## Recommended Tooling

The sample projects use:

- xUnit
- NSubstitute
- FluentAssertions

Example:

```csharp
_logger = Substitute.For<ILogger<MyHandler>>();

_service = Substitute.For<IMyService>();
```

and:

```csharp
result.HasException.Should().BeFalse();
```

Any testing framework may be used.

---

## Summary

Entra.EventHandlers is designed around direct handler testing.

Recommended approach:

1. Instantiate the handler.
2. Mock dependencies.
3. Create the event model.
4. Execute `HandleAsync()`.
5. Assert the generated response.

Most tests should focus on business behavior inside handlers rather than ASP.NET Core or Azure Functions hosting infrastructure.