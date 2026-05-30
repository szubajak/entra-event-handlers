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
- `PrefillValuesBuilder`

### ✔ Unified Entry Point

```csharp
EntraEventResponses.AttributeCollectionStart();
EntraEventResponses.AttributeCollectionSubmit();
EntraEventResponses.TokenIssuanceStart();
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
                .Allow()
                .Build());
    }
}
```

### ✔ Prefill Support

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .PrefillValues(p => p
        .With("email", "user@example.com")
        .With("country", "PL"))
    .Build();
```

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — protocol types (MIT)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions integration (BSL)

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.  
The abstractions package is MIT‑licensed.

For commercial licensing or support:  
📧 **jakub.szubarga@gmail.com**
