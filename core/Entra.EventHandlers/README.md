# Entra.EventHandlers

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package contains the **core implementation layer** for the Entra Event Handlers ecosystem.  
It builds on the MIT‑licensed **Entra.EventHandlers.Abstractions** package and provides the full
developer‑experience layer for constructing responses, composing handlers, and building
production‑ready **Microsoft Entra External ID Authentication Event Handler** extensions.

This package focuses exclusively on **External ID** events.  
Workforce events are provided separately in **Entra.EventHandlers.Workforce**.

---

## ✨ What This Package Provides

This package extends the MIT‑licensed **Entra.EventHandlers.Abstractions** with the full
implementation and developer‑experience layer for External ID authentication events.

It includes:

### ✔ Fluent, strongly‑typed response builders  
Eliminate manual JSON and ensure protocol‑correct payloads.

### ✔ Unified entry point  
A single API surface for all External ID events:

```csharp
EntraEventResponses.AttributeCollectionStart();
EntraEventResponses.AttributeCollectionSubmit();
EntraEventResponses.TokenIssuanceStart();
EntraEventResponses.EmailOtpSend();
EntraEventResponses.PasswordSubmit();
```

### ✔ Production‑ready base handler classes  
With:

- Structured logging  
- Correlation ID scoping  
- Protocol validation (`@odata.type`)  
- Execution timing  
- Consistent exception handling  

### ✔ Clean override model  
Implement your logic in `HandleCoreAsync` while the base class manages the pipeline.

### ✔ Extensibility  
Designed to integrate seamlessly with the ASP.NET Core and Azure Functions hosting adapters.

Hosting (routing, DI, request/response adapters) is provided by:

- **Entra.EventHandlers.AspNetCore**  
- **Entra.EventHandlers.AzureFunctions**

---

## 🛠 Building Responses

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .SetPrefillValues()
        .Add("email", "user@example.com")
    .Done()
    .Build();
```

---

## 🛠 Example: Implementing a Handler

```csharp
public class TokenIssuanceStartHandler(ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCoreAsync(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken)
    {
        var userId = request.Data.AuthenticationContext?.User?.Id;

        var roles = userId switch
        {
            var id when id == Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                => ["Admin", "PowerUser"],
            _ => new[] { "User" }
        };

        var customClaims = new Dictionary<string, object>
        {
            { "tenantId", "contoso-eu" },
            { "department", "Engineering" },
            { "roles", roles }
        };

        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken(customClaims)
                .Build());
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

This package includes shared sample handler implementations demonstrating how to build real
External ID handler logic using the Core package.

👉 **Sample.Common**  
Located in the repository under:  
`/samples/Sample.Common`

The sample demonstrates:

- Inheriting from the Core handler base classes  
- Using fluent response builders (`EntraEventResponses.*`)  
- Constructing block pages, prefill values, and custom claims  
- Structuring clean, production‑ready handler logic  

These samples are used by both the ASP.NET Core and Azure Functions sample applications.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers.Workforce** — Workforce‑specific event models and builders (BSL)  
- **Entra.EventHandlers.AspNetCore** — ASP.NET Core hosting adapter (BSL)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions hosting adapter (BSL)

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

See:

- [LICENSE](LICENSE) — full BSL terms  
- [LICENSE-COMMERCIAL.md](LICENSE-COMMERCIAL.md) — commercial licensing terms  

A commercial license is required for production use by organizations with more than 5 employees.

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current
and future BSL‑licensed packages.

### Commercial License Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

To purchase a license or request an invoice:

📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID Authentication Event Handlers  
and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
