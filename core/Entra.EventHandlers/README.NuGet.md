# Entra.EventHandlers

Production‑ready implementation layer for **Microsoft Entra External ID Authentication Event Handlers**.  
This package builds on top of the MIT‑licensed **Entra.EventHandlers.Abstractions** and provides fluent response builders, base handler infrastructure, and utilities for constructing custom External ID extensions.

This package focuses exclusively on **External ID** events.  
Workforce events are provided separately in **Entra.EventHandlers.Workforce**.

---

## 🚀 Features

### ✔ Fluent Response Builders

Strongly‑typed builders for constructing valid External ID responses:

- `AttributeCollectionStartResponseBuilder`
- `AttributeCollectionSubmitResponseBuilder`
- `TokenIssuanceStartResponseBuilder`
- `EmailOtpSendResponseBuilder`
- `PasswordSubmitResponseBuilder`
- `PrefillValuesBuilder`

These builders eliminate manual JSON crafting and ensure protocol‑correct payloads.

---

### ✔ Unified Entry Point

A single, discoverable API surface for creating responses:

```csharp
EntraEventResponses.AttributeCollectionStart();
EntraEventResponses.AttributeCollectionSubmit();
EntraEventResponses.TokenIssuanceStart();
EntraEventResponses.EmailOtpSend();
EntraEventResponses.PasswordSubmit();
```

---

### ✔ Supported External ID Events

- **AttributeCollectionStart**  
- **AttributeCollectionSubmit**  
- **TokenIssuanceStart**  
- **EmailOtpSend**  
- **PasswordSubmit**

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

---

### ✔ Prefill Support

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .SetPrefillValues()
        .Add("email", "user@example.com")
        .Add("country", "PL")
    .Done()
    .Build();
```

---

## 📁 Samples

Sample handler implementations are available in the repository:

👉 **Sample.Common**  
https://github.com/szubajak/entra-event-handlers/tree/main/samples/Sample.Common

These samples demonstrate:

- how to inherit from the Core handler base classes  
- how to use fluent response builders (`EntraEventResponses.*`)  
- how to construct block pages, prefill values, and custom claims  
- how to structure clean, production‑ready handler logic  

They are shared by both the ASP.NET Core and Azure Functions sample applications.

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

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID Authentication Event Handlers,
Workforce scenarios, and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
