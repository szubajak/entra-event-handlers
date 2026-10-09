# PasswordSubmit Decryption

Microsoft Entra External ID sends the user's password inside an encrypted password context during the `PasswordSubmit` custom authentication extension flow.

The `Entra.EventHandlers.Security` package provides production-ready components for decrypting this payload using certificates stored in Azure Key Vault.

For most applications, decryption is handled automatically by `PasswordSubmitHandlerBase`.

---

## Overview

The PasswordSubmit event contains:

```csharp
request.Data.EncryptedPasswordContext
```

This value contains:

- User password
- Nonce
- Optional username

encrypted using the public key configured in Microsoft Entra.

Before password validation or migration can occur, the payload must first be decrypted.

---

## Decryption Flow

```text
Microsoft Entra
        │
        ▼
EncryptedPasswordContext
        │
        ▼
Azure Key Vault Certificate
        │
        ▼
RSA Private Key
        │
        ▼
Decrypt Payload
        │
        ▼
DecryptedPasswordContext
        │
        ▼
Password Validation
        │
        ▼
Password Migration
```

---

## Installation

Install the security package:

```bash
dotnet add package Entra.EventHandlers.Security
```

---

## Configuration

Configure Azure Key Vault access:

```json
{
  "KeyVault": {
    "VaultUrl": "https://contoso.vault.azure.net/",
    "CertificateName": "password-submit-certificate"
  }
}
```

Register configuration:

```csharp
builder.Services.Configure<KeyVaultCertificateOptions>(
    builder.Configuration.GetSection(
        KeyVaultCertificateOptions.SectionName));
```

Register security services:

```csharp
builder.Services.AddEntraEventHandlersSecurity();
```

---

## Provided Components

### Password Context Decryption

Primary interface:

```csharp
IPasswordContextDecryptor
```

Provided implementation:

```csharp
KeyVaultPasswordContextDecryptor
```

Responsibilities:

- Retrieve certificates from Azure Key Vault
- Extract RSA private keys
- Decrypt encrypted password contexts
- Deserialize decrypted payloads
- Validate payload contents
- Return strongly typed models

---

### Certificate Retrieval

Primary interface:

```csharp
IKeyVaultCertificateProvider
```

Provided implementation:

```csharp
KeyVaultCertificateProvider
```

Responsibilities:

- Retrieve certificates from Azure Key Vault
- Extract RSA private keys
- Cache RSA instances
- Provide thread-safe initialization
- Reduce repeated Key Vault requests

---

### Dependency Injection

Register security services:

```csharp
builder.Services.AddEntraEventHandlersSecurity();
```

This registers the Azure Key Vault integration services used by the package.

---

## Manual vs Automatic Decryption

The package supports both approaches.

### Recommended

Use:

```csharp
PasswordSubmitHandlerBase
```

which automatically decrypts the password context before your handler executes.

### Advanced

Inject and use:

```csharp
IPasswordContextDecryptor
```

directly when building custom processing pipelines.

For most applications, automatic decryption through `PasswordSubmitHandlerBase` is the preferred approach.

---

## Using PasswordSubmitHandlerBase

The recommended approach is using:

```csharp
PasswordSubmitHandlerBase
```

The base handler automatically:

- Validates the incoming request
- Validates `@odata.type`
- Decrypts `EncryptedPasswordContext`
- Validates the decrypted payload
- Creates correlation logging scopes
- Measures execution duration
- Provides safe exception handling
- Preserves the nonce for failure scenarios

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

No manual decryption code is required.

---

## DecryptedPasswordContext

The decrypted payload is represented by:

```csharp
public sealed class DecryptedPasswordContext
{
    public string Password { get; init; }

    public string Nonce { get; init; }

    public string? Username { get; init; }
}
```

This model is supplied automatically by `PasswordSubmitHandlerBase`.

You typically do not need to deserialize or validate the payload yourself.

Example:

```csharp
var password = decrypted.Password;
var nonce = decrypted.Nonce;
var username = decrypted.Username;
```

Properties:

| Property | Description |
|-----------|-------------|
| Password | Plaintext password submitted by the user |
| Nonce | Protocol nonce provided by Microsoft Entra |
| Username | Optional username included in the encrypted payload |

---

## Azure Key Vault Certificate Provider

The package includes:

```csharp
IKeyVaultCertificateProvider
```

Implementation:

```csharp
KeyVaultCertificateProvider
```

Features:

- Azure Key Vault integration
- Certificate retrieval
- RSA private key extraction
- In-memory RSA caching
- Thread-safe initialization
- Structured logging

The RSA key is loaded only once and reused for subsequent requests.

---

## Password Validation Example

Validate credentials against a legacy identity store.

```csharp
public class PasswordSubmitHandler(
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
        var valid = await userStore.ValidatePasswordAsync(
            decrypted.Username,
            decrypted.Password,
            cancellationToken);

        return valid
            ? EntraEventResponses
                .PasswordSubmit()
                .WithNonce(decrypted.Nonce)
                .MigratePassword()
                .Build()
            : EntraEventResponses
                .PasswordSubmit()
                .WithNonce(decrypted.Nonce)
                .Block()
                .Build();
    }
}
```

In this scenario:

1. Microsoft Entra sends an encrypted password context.
2. The base handler decrypts the payload.
3. The handler receives a validated `DecryptedPasswordContext`.
4. Credentials are validated against a legacy user store.
5. The user is either migrated or blocked.

---

## Security Considerations

### Never Log Passwords

Avoid:

```csharp
logger.LogInformation(
    "Password: {Password}",
    decrypted.Password);
```

Passwords should never be logged.

---

### Never Persist Passwords

Avoid storing decrypted passwords in:

- Databases
- Logs
- Distributed caches
- Telemetry systems

Passwords should remain in memory only for the duration of the request.

---

### Always Return the Nonce

Every valid PasswordSubmit response must return:

```csharp
.WithNonce(decrypted.Nonce)
```

The nonce is required by the Microsoft Entra protocol.

---

### Prefer Azure Key Vault

Store PasswordSubmit certificates in Azure Key Vault whenever possible.

Benefits include:

- Centralized certificate management
- Managed Identity integration
- Certificate rotation support
- Improved operational security

---

## Related Documentation

### Start Here

- [Getting Started](../getting-started.md)
- [Architecture](../architecture.md)
- [Unit Testing](../testing.md)

### Events

- [AttributeCollectionStart](../events/attribute-collection-start.md)
- [AttributeCollectionSubmit](../events/attribute-collection-submit.md)
- [AspNetCore](../events/email-otp-send.md)
- [PasswordSubmit](../events/password-submit.md)
- [TokenIssuanceStart](../events/token-issuance-start.md)
- [VerifiedIdClaimValidation](../events/verified-id-claim-validation.md)

### Hosting

- [AspNetCore](../hosting/aspnetcore.md)
- [AzureFunctions](../hosting/azure-functions.md)