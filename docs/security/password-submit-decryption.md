# PasswordSubmit Decryption

Microsoft Entra External ID sends the user's password inside an encrypted password context during the PasswordSubmit custom authentication extension flow.

The Entra.EventHandlers.Security package provides a production-ready implementation for decrypting this payload using certificates stored in Azure Key Vault.

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

To validate the password, the payload must first be decrypted.

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

## Password Context Decryptor

The package provides:

```csharp
IPasswordContextDecryptor
```

Implementation:

```csharp
KeyVaultPasswordContextDecryptor
```

The decryptor:

1. Retrieves the certificate from Azure Key Vault.
2. Extracts the RSA private key.
3. Decrypts the payload.
4. Validates the payload.
5. Returns a strongly typed model.

---

## Using PasswordSubmitHandlerBase

The simplest approach is using:

```csharp
PasswordSubmitHandlerBase
```

The decrypted context is automatically supplied.

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

Example:

```csharp
var password = decrypted.Password;
var nonce = decrypted.Nonce;
var username = decrypted.Username;
```

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

Validate against a legacy user store:

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

---

### Always Return the Nonce

Every valid PasswordSubmit response must return:

```csharp
.WithNonce(decrypted.Nonce)
```

The nonce is required by the Microsoft Entra protocol.

---

### Prefer Azure Key Vault

Store PasswordSubmit certificates in Azure Key Vault rather than local machine certificate stores whenever possible.

Benefits include:

- Centralized management
- Rotation support
- Managed Identity integration
- Improved operational security

---

## Related Documentation

### Events

- ../events/password-submit.md