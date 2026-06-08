# Entra.EventHandlers

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package contains the **core implementation layer** for the Entra Event Handlers ecosystem.  
It builds on the MIT‑licensed **Entra.EventHandlers.Abstractions** package and provides the full developer experience for constructing responses, composing handlers, and building production‑ready **Microsoft Entra External ID Authentication Event Handler** extensions.

---

## ✨ What This Package Provides

This package extends the MIT‑licensed **Entra.EventHandlers.Abstractions** with the full developer‑experience layer for building Microsoft Entra External ID Authentication Event Handlers.

It provides:

- **Fluent, strongly‑typed response builders**  
  For constructing valid protocol‑correct responses without manual JSON.

- **Unified entry point**  
  A single API surface (`EntraEventResponses.*`) for all event types.

- **Production‑ready base handler classes**  
  With structured logging, validation, correlation IDs, and exception handling.

- **Clean override model**  
  Implement your logic in `HandleCore` while the base class manages the pipeline.

Hosting (routing, DI, request/response adapters) is provided by the  
**Entra.EventHandlers.AspNetCore** and **Entra.EventHandlers.AzureFunctions** packages.

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

The base class handles:

- Logging
- Validation
- Correlation IDs
- Exception handling

---

## 📁 Samples

This package includes a set of sample handler implementations demonstrating how to build real Entra Event Handler logic using the Core package:

- **Sample.Common** — shared sample handlers used by both the ASP.NET Core and Azure Functions samples.

The sample shows:

- How to implement handlers by inheriting from the base classes  
- How to use fluent response builders (`EntraEventResponses.*`)  
- How to construct block pages, prefill values, and custom claims  
- How to structure clean, production‑ready handler logic  

You can find the sample handlers in the repository under:  
[Sample.Common](../../samples/Sample.Common) project.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers** — core implementation layer (BSL)
- **Entra.EventHandlers.AspNetCore** — ASP.NET Core hosting adapter (BSL)  

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

## 📚 Documentation

Full documentation, examples, and production templates will be available in the
main repository as the ecosystem evolves.
