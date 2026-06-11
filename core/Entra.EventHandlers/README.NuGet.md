# Entra.EventHandlers

Production‑ready implementation layer for Microsoft Entra External ID Authentication Event Handlers.  
This package builds on top of the MIT‑licensed **Entra.EventHandlers.Abstractions** and provides fluent response builders, base handler infrastructure, and utilities for constructing custom extensions.

---

## 🚀 Features

### ✔ Fluent Response Builders

Strongly‑typed builders for constructing valid Entra responses:

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

### ✔ Base Handler Infrastructure

Includes:

- Structured logging  
- Correlation scoping  
- Execution timing  
- Protocol validation (`@odata.type`)  
- Consistent exception handling  
- Clean override point (`HandleCore`)  

```csharp
public class TokenIssuanceStartHandler(ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCore(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken)
    {
        // Extract user ID (GUID)
        var userId = request.Data.AuthenticationContext?.User?.Id;

        // Example: determine roles based on user ID
        var roles = userId switch
        {
            // Example: special admin GUID
            var id when id == Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                => ["Admin", "PowerUser"],

            // Default
            _ => new[] { "User" }
        };

        // Example: add custom claims
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

- **Entra.EventHandlers.Abstractions** — protocol types (MIT)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions integration (BSL)
- **Entra.EventHandlers.AspNetCore** — ASP.NET Core hosting adapter (BSL)

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

See:
- [LICENSE](https://github.com/szubajak/entra-event-handlers/blob/main/core/Entra.EventHandlers/LICENSE)
- [LICENSE-COMMERCIAL.md](https://github.com/szubajak/entra-event-handlers/blob/main/core/Entra.EventHandlers/LICENSE-COMMERCIAL.md)

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

For a deeper look into Microsoft Entra External ID Authentication Event Handlers
and the design of this ecosystem, see the full article:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
