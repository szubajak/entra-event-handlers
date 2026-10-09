# Entra.EventHandlers.Security

Security extensions for Microsoft Entra External ID authentication event handlers.

This package provides production-ready components for handling security-sensitive scenarios in the Entra.EventHandlers ecosystem, including PasswordSubmit payload decryption using certificates stored in Azure Key Vault.

## Installation

```bash
dotnet add package Entra.EventHandlers.Security
```

## Why This Package Exists

The Microsoft Entra External ID `PasswordSubmit` event sends the user's password inside an encrypted password context.

Decrypting this payload requires:

- Secure certificate management
- RSA private key extraction
- JWE/JWT decryption
- Payload deserialization
- Payload validation
- Secure cryptographic handling

This package provides production-ready implementations for these requirements.

Most applications using `PasswordSubmit` never need to manually decrypt the payload.

When using `PasswordSubmitHandlerBase`, the encrypted password context is automatically decrypted and provided as a strongly typed `DecryptedPasswordContext` instance.

---

## Features

### Password Context Decryption

Decrypt encrypted password contexts received during the `PasswordSubmit` event.

Primary interface:

```csharp
IPasswordContextDecryptor
```

Provided implementation:

```csharp
KeyVaultPasswordContextDecryptor
```

---

### Azure Key Vault Certificate Integration

Load certificates stored in Azure Key Vault.

Primary interface:

```csharp
IKeyVaultCertificateProvider
```

Provided implementation:

```csharp
KeyVaultCertificateProvider
```

---

### Dependency Injection Integration

Simple registration using standard .NET dependency injection.

```csharp
builder.Services.AddEntraEventHandlersSecurity();
```

---

### Secure RSA Key Caching

The RSA private key is:

- Loaded only once
- Cached in memory
- Reused across requests
- Protected by thread-safe initialization

This minimizes Azure Key Vault traffic and reduces request latency.

---

## Supported Scenario

This package currently focuses on the Microsoft Entra External ID:

```text
PasswordSubmit
```

custom authentication extension event.

Typical flow:

```text
Microsoft Entra
        │
        ▼
PasswordSubmit Event
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
DecryptedPasswordContext
        │
        ▼
Password Validation
        │
        ▼
Password Migration
```

---

## Configuration

Configure access to the Azure Key Vault certificate.

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

## PasswordSubmit Integration

The primary use case for this package is the `PasswordSubmit` event.

The security package integrates directly with:

```csharp
PasswordSubmitHandlerBase
```

Once an `IPasswordContextDecryptor` implementation is supplied, the encrypted password context is automatically decrypted before your business logic executes.

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

The handler receives a fully validated:

```csharp
DecryptedPasswordContext
```

without any manual decryption code.

The base handler automatically:

- Validates the request
- Decrypts the encrypted password context
- Validates the decrypted payload
- Handles structured logging
- Creates correlation scopes
- Preserves the nonce
- Generates safe responses when failures occur

---

## Decrypted Password Context

The decrypted payload is represented by:

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
| Password | Plaintext password submitted by the user |
| Nonce | Protocol nonce provided by Microsoft Entra |
| Username | Optional username associated with the password |

Example:

```csharp
var username = decrypted.Username;
var password = decrypted.Password;
var nonce = decrypted.Nonce;
```

Values are validated before being returned to your handler.

---

## Azure Key Vault Certificate Provider

The package includes a certificate provider that retrieves PKCS#12 certificates from Azure Key Vault and extracts RSA private keys.

Primary interface:

```csharp
IKeyVaultCertificateProvider
```

Implementation:

```csharp
KeyVaultCertificateProvider
```

Features:

- Azure Key Vault integration
- Certificate loading
- RSA key extraction
- In-memory caching
- Thread-safe initialization
- Structured logging

The RSA private key is extracted only once and reused across requests.

---

## Example

Validate credentials against a legacy identity store.

```csharp
public class PasswordSubmitHandler(
    ILogger<PasswordSubmitHandler> logger,
    IPasswordContextDecryptor decryptor)
    : PasswordSubmitHandlerBase(logger, decryptor)
{
    protected override async Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken = default)
    {
        var validCredentials =
            await ValidateCredentialsAsync(
                decrypted.Username,
                decrypted.Password);

        return validCredentials
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

In this example:

1. Microsoft Entra sends an encrypted password context.
2. The base handler decrypts the payload.
3. Business logic receives a strongly typed `DecryptedPasswordContext`.
4. The password is validated against a legacy identity store.
5. A migration or block response is returned.

---

## Security Considerations

### Never Log Passwords

Avoid:

```csharp
_logger.LogInformation(
    "Password: {Password}",
    decrypted.Password);
```

Passwords should never be logged.

---

### Never Persist Passwords

Avoid storing decrypted passwords in:

- Databases
- Logs
- Telemetry systems
- Distributed caches

Passwords should exist only for the duration of the request.

---

### Always Return the Nonce

PasswordSubmit responses must include:

```csharp
.WithNonce(decrypted.Nonce)
```

The nonce is required by the Microsoft Entra protocol.

---

### Prefer Azure Key Vault

Store PasswordSubmit certificates in Azure Key Vault whenever possible.

Benefits include:

- Centralized management
- Managed Identity integration
- Certificate rotation
- Improved operational security

---

## Documentation

### Start Here

- [Getting Started](../../docs/getting-started.md)
- [Architecture](../../docs/architecture.md)
- [Unit Testing](docs/testing.md)

### Security

- ../docs/security/password-submit-decryption.md

### Related Events

- ../docs/events/password-submit.md

### Hosting

- ../docs/hosting/aspnetcore.md
- ../docs/hosting/azure-functions.md

---

## AI Discovery

This repository includes AI-friendly documentation and metadata:

- `llms.txt`
- `docs/`

AI assistants should begin with:

1. `llms.txt`
2. `docs/getting-started.md`
3. `docs/events/password-submit.md`
4. `docs/security/password-submit-decryption.md`

---

## Related Packages

| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Public contracts and protocol models |
| Entra.EventHandlers | External ID implementation layer |
| Entra.EventHandlers.Workforce | Workforce implementation layer |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting |
| Entra.EventHandlers.AzureFunctions | Azure Functions hosting |

---

## License

This package is licensed under the Business Source License (BSL).

The Entra.EventHandlers.Abstractions package is licensed under MIT and may be used freely.

See the repository for licensing details and commercial licensing information.

---

## Further Reading

Microsoft Entra External ID Just-In-Time Password Migration

https://learn.microsoft.com/en-us/entra/external-id/customers/how-to-migrate-passwords-just-in-time

Entra External ID .NET Handlers Deep Dive

https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437