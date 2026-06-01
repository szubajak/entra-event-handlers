# Entra.EventHandlers

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package contains the full implementation layer for the Entra Event
Handlers ecosystem. It builds on top of the MIT‑licensed
**Entra.EventHandlers.Abstractions** package and provides higher‑level
functionality for constructing responses, composing handlers, and building
production‑ready authentication event extensions.

---

## ✨ What This Package Provides

This package extends the abstractions with implementation features such as:

### ✔ Fluent response builders

Strongly‑typed, ergonomic builders for constructing valid Entra responses:

- `AttributeCollectionStartResponseBuilder`
- `AttributeCollectionSubmitResponseBuilder`
- `TokenIssuanceStartResponseBuilder`
- `PrefillValuesBuilder` (for attribute prefill scenarios)

These builders eliminate manual JSON crafting and ensure protocol‑correct
response payloads.

### ✔ Unified entry point

A single, discoverable API surface for creating responses:

```csharp
EntraEventResponses.AttributeCollectionStart();
EntraEventResponses.AttributeCollectionSubmit();
EntraEventResponses.TokenIssuanceStart();
```

### ✔ Base handler infrastructure

Production‑ready base classes that provide:

- Structured logging  
- Correlation scoping  
- Execution timing  
- Protocol‑level validation (`@odata.type`)  
- Consistent exception handling  
- A clean override point (`HandleCore`)  

Example:

```csharp
public abstract class AttributeCollectionStartHandlerBase
    : IAttributeCollectionStartHandler
{
    protected abstract Task<AttributeCollectionStartResponse> HandleCore(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken);
}
```

These base handlers remove boilerplate and ensure consistent behavior across all
custom extensions.

### ✔ Extensibility pipeline (in progress)

Future versions will include:

- Handler composition and routing  
- Execution pipeline components  
- Validation helpers  
- Telemetry hooks  
- Test utilities  
- Integration helpers for Azure Functions and other hosts  

---

## 🧩 Relationship to the Abstractions Package

This package depends on:

- **Entra.EventHandlers.Abstractions** (MIT)

The abstractions define the protocol and public contract.  
This package provides the implementation and developer experience on top of it.

You are free to use the abstractions in any project (open‑source or commercial).  
This implementation package is licensed under the **Business Source License (BSL)**.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions integration (BSL)  
  - Automatic request/response handling  
  - DI wiring  
  - Minimal boilerplate for production deployments  

These packages will be published to NuGet soon.

---

## 🛠 Example: Building a Response

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .ShowBlockPage("Error", "Unexpected error occurred.")
    .Build();
```

With prefill:

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .PrefillValues(p => p
        .With("email", "user@example.com")
        .With("country", "PL"))
    .Build();
```

---

## 🛠 Example: Implementing a Handler

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

The base class automatically provides:

- CorrelationId logging  
- EventType/EventName scoping  
- Duration measurement  
- Validation  
- Exception handling  

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
