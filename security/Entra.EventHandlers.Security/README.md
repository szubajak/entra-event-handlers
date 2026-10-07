# Entra.EventHandlers.Security
 
Security utilities for Microsoft Entra External ID custom authentication extensions.
 
This package provides production-ready components for handling security-related operations in Entra event handlers, including PasswordSubmit payload decryption using certificates stored in Azure Key Vault.
 
## Installation
 
```bash
dotnet add package Entra.EventHandlers.Security
```
 
## Why This Package Exists
 
The Microsoft Entra External ID `PasswordSubmit` event sends the user's password inside an encrypted password context.
 
Decrypting this payload requires:
 
- Accessing the private certificate used by Entra
- Retrieving certificate material securely
- Extracting the RSA private key
- Performing JWT/JWE decryption
- Deserializing and validating the payload
 
This package provides a reusable implementation of that workflow.
 
## Features
 
### Password Context Decryption
 
Decrypt encrypted password contexts received during the `PasswordSubmit` event.
 
```csharp
IPasswordContextDecryptor
```
 
Default implementation:
 
```csharp
KeyVaultPasswordContextDecryptor
```
 
### Azure Key Vault Certificate Integration
 
Load certificates stored in Azure Key Vault.
 
```csharp
IKeyVaultCertificateProvider
```
 
Default implementation:
 
```csharp
KeyVaultCertificateProvider
```
 
### Dependency Injection Integration
 
Simple registration using standard .NET dependency injection.
 
```csharp
builder.Services.AddEntraEventHandlersSecurity();
```
 
### Secure Key Caching
 
The RSA private key is:
 
- Loaded only once
- Cached in memory
- Reused across requests
- Protected by thread-safe initialization
 
This minimizes Azure Key Vault traffic and reduces request latency.
 
---
 
# Supported Scenario
 
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
Encrypted Password Context
│
▼
Azure Key Vault Certificate
│
▼
RSA Private Key
│
▼
Decrypted Password Payload
│
▼
Password Validation or Migration
```
 
---
 
# Configuration
 
Configure access to the Azure Key Vault certificate.
 
```json
{
"KeyVault": {
"VaultUrl": "https://contoso.vault.azure.net/",
"CertificateName": "password-submit-certificate"
}
}
```
 
Bind configuration:
 
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
 
# Password Context Decryption
 
Inject the decryptor into your handler:
 
```csharp
public class PasswordSubmitHandler(
ILogger<PasswordSubmitHandler> logger,
IPasswordContextDecryptor decryptor)
: PasswordSubmitHandlerBase(logger)
{
private readonly IPasswordContextDecryptor _decryptor = decryptor;
 
protected override async Task<PasswordSubmitResponse> HandleCoreAsync(
PasswordSubmitEvent request,
CancellationToken cancellationToken = default)
{
var context = await _decryptor.DecryptAsync(
request.Data.PasswordContext,
cancellationToken);
 
var username = context.Username;
var password = context.Password;
 
// Validate credentials
// Migrate user
// Call external identity provider
 
return EntraEventResponses
.PasswordSubmit()
.ValidateCredentials(
PasswordValidationStatus.Valid)
.Build();
}
}
```
 
---
 
# Decrypted Payload
 
The decrypted context contains:
 
```csharp
public sealed class DecryptedPasswordContext
{
public string Password { get; init; }
 
public string Nonce { get; init; }
 
public string? Username { get; init; }
}
```
 
Values are validated before being returned.
 
---
 
# Azure Key Vault Certificate Provider
 
The package includes a certificate provider that retrieves PKCS#12 certificates from Azure Key Vault and extracts the RSA private key.
 
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
- RSA extraction
- In-memory caching
- Thread-safe initialization
- Structured logging
 
---
 
# Example
 
Complete PasswordSubmit handler:
 
```csharp
public class PasswordSubmitHandler(
ILogger<PasswordSubmitHandler> logger,
IPasswordContextDecryptor decryptor)
: PasswordSubmitHandlerBase(logger)
{
protected override async Task<PasswordSubmitResponse> HandleCoreAsync(
PasswordSubmitEvent request,
CancellationToken cancellationToken = default)
{
var passwordContext = await decryptor.DecryptAsync(
request.Data.PasswordContext,
cancellationToken);
 
bool validCredentials =
await ValidateCredentialsAsync(
passwordContext.Username,
passwordContext.Password);
 
return EntraEventResponses
.PasswordSubmit()
.ValidateCredentials(
validCredentials
? PasswordValidationStatus.Valid
: PasswordValidationStatus.Invalid)
.Build();
}
}
```
 
---
 
# Documentation
 
Start here:
 
- Getting Started
https://github.com/szubajak/entra-eventhandlers/blob/main/docs/getting-started.md
 
- Architecture
https://github.com/szubajak/entra-eventhandlers/blob/main/docs/architecture.md
 
- PasswordSubmit Event Documentation
https://github.com/szubajak/entra-eventhandlers/blob/main/docs/events/password-submit.md
 
- Samples
https://github.com/szubajak/entra-eventhandlers/tree/main/samples
 
## AI Discovery
 
AI assistants should begin with:
 
1. `llms.txt`
2. `docs/getting-started.md`
3. `docs/events/password-submit.md`
 
---
 
# Related Packages
 
| Package | Purpose |
|----------|----------|
| Entra.EventHandlers.Abstractions | Event contracts and protocol models |
| Entra.EventHandlers | Event handlers and fluent response builders |
| Entra.EventHandlers.AspNetCore | ASP.NET Core hosting |
| Entra.EventHandlers.AzureFunctions | Azure Functions hosting |
 
---
 
# License
 
This package is licensed under the Business Source License (BSL).
 
See the repository for complete licensing details.
 
---
 
# Further Reading
 
**Microsoft Entra External ID Password Migration**
 
https://learn.microsoft.com/en-us/entra/external-id/customers/how-to-define-custom-claims-customization
 
**Entra External ID .NET Handlers Deep Dive**
 
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437