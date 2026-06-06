# Entra.EventHandlers

Production‑ready implementation layer for Microsoft Entra Authentication Event
Handlers. This package builds on top of the MIT‑licensed
**Entra.EventHandlers.Abstractions** and provides fluent response builders,
base handler infrastructure, and utilities for constructing custom extensions.

---

## 🚀 Features

### ✔ Fluent Response Builders
Strongly‑typed builders for constructing valid Entra responses:

- `AttributeCollectionStartResponseBuilder`
- `AttributeCollectionSubmitResponseBuilder`
- `TokenIssuanceStartResponseBuilder`
- `EmailOtpSendResponseBuilder`
- `PrefillValuesBuilder`

### ✔ Unified Entry Point

```csharp
EntraEventResponses.AttributeCollectionStart();
EntraEventResponses.AttributeCollectionSubmit();
EntraEventResponses.TokenIssuanceStart();
EntraEventResponses.EmailOtpSend();
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
public class MyStartHandler : AttributeCollectionStartHandlerBase
{
    public MyStartHandler(ILogger<MyStartHandler> logger) : base(logger) {}

    protected override Task<AttributeCollectionStartResponse> HandleCore(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .ContinueWithDefaultBehavior()
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
