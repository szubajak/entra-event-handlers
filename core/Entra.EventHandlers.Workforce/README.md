# Entra.EventHandlers.Workforce

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package contains the **Workforce‑specific event models, response builders, and handler base classes** for the Entra Event Handlers ecosystem.  
It extends the MIT‑licensed **Entra.EventHandlers.Abstractions** package with strongly‑typed request/response types and fluent builders for **Microsoft Entra Workforce account recovery flows**, including the **VerifiedIdClaimValidation** event.

This package is designed to be used together with the hosting adapters:

- **Entra.EventHandlers.AspNetCore**  
- **Entra.EventHandlers.AzureFunctions**

It operates **in parallel** to the core implementation package (**Entra.EventHandlers**) and does not depend on it.

---

## ✨ What This Package Provides

This package adds Workforce‑specific capabilities to the Entra Event Handlers ecosystem:

- **VerifiedIdClaimValidation event models**  
  Strongly‑typed request/response types for Workforce account recovery flows.

- **Fluent response builders**  
  For constructing `Pass` or `Failed` validation outcomes without manual JSON.

- **Unified Workforce entry point**  
  `EntraWorkforceEventResponses.VerifiedIdClaimValidation()`  
  for discoverable, guided response construction.

- **Production‑ready base handler class**  
  `VerifiedIdClaimValidationHandlerBase`  
  with structured logging, validation, correlation IDs, and exception handling.

This package does **not** include hosting logic — routing, DI, and request/response adapters are provided by the ASP.NET Core and Azure Functions hosting packages.

---

## 🛠 Building Responses

```csharp
return EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
    .Pass()
    .Build();
```

Or return a failed validation:

```csharp
return EntraWorkforceEventResponses
    .VerifiedIdClaimValidation()
    .Failed(["employeeId", "department"])
    .Build();
```

---

## 🛠 Example: Implementing a Workforce Handler

```csharp
public class VerifiedIdHandler(ILogger<VerifiedIdHandler> logger)
    : VerifiedIdClaimValidationHandlerBase(logger)
{
    protected override Task<VerifiedIdClaimValidationResponse> HandleCoreAsync(
        VerifiedIdClaimValidationEvent request,
        CancellationToken cancellationToken)
    {
        // Example: validate claims against authoritative HR data
        var failedClaims = new List<string>();

        if (!HrSystem.IsValidEmployeeId(request.Data.Claims.EmployeeId))
            failedClaims.Add("employeeId");

        if (!HrSystem.IsValidDepartment(request.Data.Claims.Department))
            failedClaims.Add("department");

        return Task.FromResult(
            failedClaims.Count == 0
                ? EntraWorkforceEventResponses.VerifiedIdClaimValidation().Pass().Build()
                : EntraWorkforceEventResponses.VerifiedIdClaimValidation().Failed(failedClaims).Build());
    }
}
```

The base class handles:

- Logging  
- Validation  
- Correlation IDs  
- Exception handling  
- Execution timing  

---

## 📁 Samples

The repository includes sample implementations demonstrating how to use the Workforce package together with the hosting adapters.

The samples show:

- How to implement Workforce handlers  
- How to use the fluent Workforce response builders  
- How to integrate Workforce events into ASP.NET Core and Azure Functions  
- How to structure clean, production‑ready handler logic  

You can find the sample handlers in the repository under:  
[Sample.Common](../../samples/Sample.Common)

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

- [LICENSE](LICENSE) — full BSL terms  
- [LICENSE-COMMERCIAL.md](LICENSE-COMMERCIAL.md) — commercial licensing terms  

A commercial license is required for production use by organizations with more than 5 employees.

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current and future BSL‑licensed packages.

### Commercial License Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

To purchase a license or request an invoice:

📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID and Workforce Authentication Event Handlers  
and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
