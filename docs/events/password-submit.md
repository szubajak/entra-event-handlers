# Password Submit

The `PasswordSubmit` event is raised by Microsoft Entra External ID during a Just-In-Time (JIT) password migration flow.

This event allows your application to:

- Validate a user's existing password against an external identity store
- Migrate passwords during sign-in
- Update stored passwords
- Request a retry
- Block authentication

Unlike other events, the password is not directly included in the request payload. Instead, Microsoft Entra provides an encrypted password context that must be decrypted before validation can occur.

---

## When is this event called?

Microsoft Entra invokes the `PasswordSubmit` custom authentication extension when a user attempts to authenticate during a password migration scenario.

Typical use cases include:

- Migrating users from a legacy identity provider
- Migrating users from a custom membership database
- Gradually moving users into Microsoft Entra External ID
- Just-in-time password migration
- Password verification against external systems

---

## Password Migration Flow

The overall flow is:

```text
User Sign-In
        │
        ▼
Microsoft Entra
        │
        ▼
PasswordSubmit Event
        │
        ▼
Encrypted Password Context
        │
        ▼
Decrypt Password Context
        │
        ▼
Validate Password
        │
        ▼
Choose Response Action
        │
        ▼
Microsoft Entra
```

---

## Handler Base Class

Implement `PasswordSubmitHandlerBase` and override `HandleCoreAsync`.

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

Unlike other event handlers, `HandleCoreAsync` receives:

```csharp
DecryptedPasswordContext decrypted
```

The framework decrypts and validates the encrypted password payload before invoking your business logic.

---

## Request Model

The incoming request is represented by:

```csharp
PasswordSubmitEvent
```

The event includes:

```csharp
request.Data.EncryptedPasswordContext
```

This value contains:

- User password
- Nonce
- Optional username

encrypted using the certificate configured for the PasswordSubmit flow.

---

## Decrypted Password Context

After decryption the framework supplies a fully validated:

```csharp
DecryptedPasswordContext
```

Model:

```csharp
public sealed class DecryptedPasswordContext
{
    public string Password { get; init; }

    public string Nonce { get; init; }

    public string? Username { get; init; }
}
```

Properties:

| Property | Description |
|-----------|-------------|
| Password | The plaintext password submitted by the user |
| Nonce | Protocol nonce that must be returned unchanged |
| Username | Optional username provided by Entra |

Example:

```csharp
var password = decrypted.Password;
var nonce = decrypted.Nonce;
var username = decrypted.Username;
```

---

## Security Considerations

The password contained within:

```csharp
DecryptedPasswordContext.Password
```

should be treated as highly sensitive.

Avoid:

- Logging passwords
- Storing passwords
- Caching passwords
- Returning passwords in exceptions

The password should exist only for the duration of the request.

---

## Password Context Decryption

The handler base class requires:

```csharp
IPasswordContextDecryptor
```

which is responsible for decrypting the encrypted password context.

```csharp
public class PasswordSubmitHandler(
    ILogger<PasswordSubmitHandler> logger,
    IPasswordContextDecryptor decryptor)
    : PasswordSubmitHandlerBase(logger, decryptor)
{
}
```

Applications may:

- Provide their own implementation
- Use the implementation included in `Entra.EventHandlers.Security`

The security package includes:

- Azure Key Vault certificate integration
- Password context decryption
- RSA private key extraction
- Certificate caching

---

## Nonce Handling

Microsoft Entra includes a nonce in the encrypted password context.

This nonce must always be returned in the response.

Example:

```csharp
return EntraEventResponses
    .PasswordSubmit()
    .WithNonce(decrypted.Nonce)
    .MigratePassword()
    .Build();
```

Without the nonce, the response is invalid.

The response builder enforces this requirement.

---

## Response Builder

Use:

```csharp
EntraEventResponses.PasswordSubmit()
```

to create `PasswordSubmitResponse` instances.

All responses require:

```csharp
.WithNonce(...)
```

before selecting an action.

---

## Available Responses

The event supports the following actions.

| Action | Purpose |
|----------|----------|
| MigratePassword | Password validated successfully and should be migrated |
| UpdatePassword | Password should be updated |
| Retry | User should retry authentication |
| Block | Authentication should be blocked |

---

## MigratePassword

Indicates that the supplied password is valid and should be migrated into Microsoft Entra.

### When to use

Use this response when:

- The password is valid
- The external account exists
- Authentication succeeds
- Migration should occur

### Example

```csharp
return EntraEventResponses
    .PasswordSubmit()
    .WithNonce(decrypted.Nonce)
    .MigratePassword()
    .Build();
```

### Result

Microsoft Entra migrates the password and continues the sign-in process.

---

## UpdatePassword

Indicates that the password should be updated.

### When to use

Use this action when:

- An external password update is required
- Additional migration scenarios exist
- Password synchronization workflows are implemented

### Example

```csharp
return EntraEventResponses
    .PasswordSubmit()
    .WithNonce(decrypted.Nonce)
    .UpdatePassword()
    .Build();
```

---

## Retry

Requests that the user retry authentication.

### When to use

Use this action when:

- Temporary failures occur
- External services are unavailable
- Authentication should be attempted again

### Example

```csharp
return EntraEventResponses
    .PasswordSubmit()
    .WithNonce(decrypted.Nonce)
    .Retry()
    .Build();
```

---

## Block

Blocks authentication.

### When to use

Use this action when:

- The password is invalid
- The account is disabled
- Authentication should not continue
- Security policies require a failure

### Example

```csharp
return EntraEventResponses
    .PasswordSubmit()
    .WithNonce(decrypted.Nonce)
    .Block()
    .Build();
```

### Result

Authentication is denied.

---

## Real-World Example

Validate a password against a legacy user store.

```csharp
public sealed class PasswordSubmitHandler(
    ILogger<PasswordSubmitHandler> logger,
    IPasswordContextDecryptor decryptor,
    ILegacyUserStore userStore)
    : PasswordSubmitHandlerBase(logger, decryptor)
{
    protected override async Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken = default)
    {
        var isValid = await userStore.ValidatePasswordAsync(
            decrypted.Username,
            decrypted.Password,
            cancellationToken);

        return EntraEventResponses
            .PasswordSubmit()
            .WithNonce(decrypted.Nonce)
            .MigratePassword(isValid)
            .Build();
    }
}
```

A more explicit version:

```csharp
if (isValid)
{
    return EntraEventResponses
        .PasswordSubmit()
        .WithNonce(decrypted.Nonce)
        .MigratePassword()
        .Build();
}

return EntraEventResponses
    .PasswordSubmit()
    .WithNonce(decrypted.Nonce)
    .Block()
    .Build();
```

---

## Exception Handling

`PasswordSubmitHandlerBase` provides additional behavior compared to other handler base classes.

If an exception occurs after successful decryption:

```text
Decrypt Password
        ▼
Exception
        ▼
Automatic Block Response
```

The framework automatically returns:

```csharp
.PasswordSubmit()
.WithNonce(decrypted.Nonce)
.Block()
```

This guarantees protocol correctness by preserving the nonce.

---

## Testing

PasswordSubmit handlers can be tested directly.

Example:

```csharp
var response = await handler.HandleAsync(
    request,
    cancellationToken);
```

Mock:

- IPasswordContextDecryptor
- External user stores
- Validation services

This allows testing:

- Password validation logic
- Migration decisions
- Failure paths
- Response generation

without any hosting infrastructure.

---

## Best Practices

### Never Log Passwords

Avoid:

```csharp
_logger.LogInformation(
    "Password: {Password}",
    decrypted.Password);
```

Never log plaintext passwords.

---

### Always Return the Nonce

Good:

```csharp
.WithNonce(decrypted.Nonce)
```

Every valid response must contain the nonce.

---

### Keep Validation Fast

Avoid:

- Long-running queries
- Slow external integrations
- Expensive computations

Authentication flows should remain responsive.

---

### Prefer Entra.EventHandlers.Security

When using Azure Key Vault certificates, consider using:

```text
Entra.EventHandlers.Security
```

instead of implementing decryption infrastructure yourself.

---

### Return Exactly One Action

Every `PasswordSubmit` response returns a single action.

Choose one:

- `MigratePassword()`
- `UpdatePassword()`
- `Retry()`
- `Block()`

---

## Summary

The Microsoft Entra External ID `PasswordSubmit` event enables Just-In-Time password migration and external password validation.

Required response component:

| Component | Purpose |
|------------|----------|
| WithNonce | Returns the protocol nonce provided by Microsoft Entra |

Supported actions:

| Response | Purpose |
|-----------|----------|
| MigratePassword | Migrate a valid password |
| UpdatePassword | Update password information |
| Retry | Retry authentication |
| Block | Deny authentication |

This event is commonly used for legacy identity migration, password synchronization, and staged migrations into Microsoft Entra External ID.

## Related Documentation

### Security

- ../security/password-submit-decryption.md