# Entra.EventHandlers.AspNetCore

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package provides the **ASP.NET Core hosting adapter** for the Entra Event Handlers ecosystem. It enables production‑ready **Microsoft Entra External ID Authentication Event Handler** extensions to run inside ASP.NET Core with minimal boilerplate, full DI support, structured error handling, and complete testability.

---

## ✨ What This Package Provides

### ✔ Full routing pipeline

The recommended hosting model is the **router endpoint**, powered by `EntraEventRouterEndpointBase`. It provides:

- Automatic request deserialization  
- Automatic handler resolution  
- Automatic handler invocation  
- Automatic response serialization  
- Structured error mapping  
- Logging for expected and unexpected exceptions  

This allows a **single ASP.NET Core endpoint** to host **multiple Entra event types** cleanly.

---

## 🧩 Minimal Router Endpoint

```csharp
public sealed class EntraRouterEndpoint : EntraEventRouterEndpointBase
{
    public EntraRouterEndpoint(
        ILogger<EntraEventRouterEndpointBase> logger,
        IEntraEventHandlerResolver resolver,
        IRequestAdapter requestAdapter,
        IResponseAdapter responseAdapter)
        : base(logger, resolver, requestAdapter, responseAdapter) {}

    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/entra/router", Invoke);
    }
}
```

---

## 🛠 Dependency Injection

Register all required components with a single call:

```csharp
services.AddEntraEventHandlers();
```

This automatically registers:
- Request/response adapters
- Handler resolver
- All handlers implementing `IEntraEventHandler<,>`

---

## 🧠 Handler Resolution

Handlers are resolved dynamically based on the event type:

```csharp
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler Resolve(Type eventType);
}
```

This enables multi‑event hosting behind a single ASP.NET Core endpoint.

---

## 📦 Optional: Single‑Event Base Classes

You can use the simple single‑event hosting model:

```csharp
public sealed class TokenIssuanceStartEndpoint : TokenIssuanceStartEndpointBase
{
    public TokenIssuanceStartEndpoint(
        ITokenIssuanceStartHandler handler,
        IRequestAdapter requestAdapter,
        IResponseAdapter responseAdapter)
        : base(handler, requestAdapter, responseAdapter) {}

    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/entra/tokenissuancestart", Invoke);
    }
}
```

---

## 🧪 Testing

The router and adapters are fully testable thanks to the abstractions.  
Unit tests are available in the  
[Entra.EventHandlers.AspNetCore.UnitTests](../Entra.EventHandlers.AspNetCore.UnitTests) project.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers** — core implementation layer (BSL)
- **Entra.EventHandlers.AzureFunctions** — Azure Functions hosting adapter (BSL)

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

See:

- **LICENSE** — full BSL terms  
- **LICENSE-COMMERCIAL.md** — commercial licensing terms  

A commercial license is required for production use by organizations with more than 5 employees.

### Commercial License Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

To purchase a license or request an invoice:

📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📚 Documentation

Full documentation, examples, and production templates will be available in the main repository as the ecosystem evolves.
