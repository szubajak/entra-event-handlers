# Entra.EventHandlers.Workforce

Workforce‑specific event models, fluent builders, and handler base classes for  
**Microsoft Entra Workforce account recovery extensions**.

This package builds on top of the MIT‑licensed **Entra.EventHandlers.Abstractions**  
and provides the strongly‑typed request/response types and developer‑experience layer  
for the **VerifiedIdClaimValidation** event used during Workforce account recovery.

---

## 🚀 Features

### ✔ Workforce Event Models

Strongly‑typed request and response types for:

- `VerifiedIdClaimValidationEvent`
- `VerifiedIdClaimValidationResponse`
- `VerifiedIdClaimValidationPassAction`
- `VerifiedIdClaimValidationFailedAction`

These models mirror the official Microsoft Entra Workforce event schema.

---

### ✔ Fluent Response Builders

A guided, strongly‑typed API for constructing valid Workforce responses:

- `VerifiedIdClaimValidationResponseBuilder`
- `FailedClaimsBuilder`

Example:

```csharp
return EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
    .Pass()
    .Build();
```

Or return failed claims:

```csharp
return EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
    .Failed(["employeeId", "department"])
    .Build();
```

---

### ✔ Unified Workforce Entry Point

A single, discoverable API surface:

```csharp
EntraWorkforceEventResponses.VerifiedIdClaimValidation();
```

---

### ✔ Base Handler Infrastructure

Includes:

- Structured logging  
- Correlation scoping  
- Execution timing  
- Protocol validation (`@odata.type`)  
- Consistent exception handling  
- Clean override point (`HandleCoreAsync`)  

```csharp
public class VerifiedIdHandler(ILogger<VerifiedIdHandler> logger)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
    protected override Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken)
    {
        // Custom validation logic here
        return Task.FromResult(
            EntraWorkforceEventResponses
                .VerifiedIdClaimValidation()
                .Pass()
                .Build());
    }
}
```

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers** — core implementation layer for External ID (BSL)  
- **Entra.EventHandlers.AspNetCore** — ASP.NET Core hosting adapter (BSL)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions hosting adapter (BSL)

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

See:

- `LICENSE` — full BSL terms  
- `LICENSE-COMMERCIAL.md` — commercial licensing terms  

A commercial license is required for production use by organizations with more than 5 employees.

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current and future BSL‑licensed packages.

### Commercial License Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

For commercial licensing or support:  
📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.
