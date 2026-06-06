# Entra.EventHandlers.AspNetCore

**ASP.NET Core hosting adapter for Microsoft Entra External ID Authentication Event Handlers.**  
Provides minimal‑boilerplate hosting, full DI support, structured error handling, and complete testability.

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

---

## ✨ Features

- 🚀 **Single Endpoint → Multiple Entra Event Types**  
- 🔄 **Automatic request deserialization & response serialization**  
- 🧩 **Dynamic handler resolution via DI**  
- 🛡 **Structured error mapping (400/500)**  
- 🧪 **Fully unit‑testable**  
- 🪶 **Minimal boilerplate**

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

## 📦 Optional: Single‑Event Base Classes

If you prefer one endpoint per event type:

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

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

A commercial license is required for production use by organizations with more than 5 employees.

### Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📚 Documentation

Full documentation, examples, and production templates will be available in the main repository as the ecosystem evolves.
