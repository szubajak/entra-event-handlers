# Entra.EventHandlers.AzureFunctions

**License:** Business Source License (BSL)
**Author:** Jakub Szubarga (Szubarga.NET)

This package provides the **Azure Functions hosting adapter** for the Entra Event Handlers ecosystem.
It enables production‑ready **Microsoft Entra External ID and Workforce Authentication Event Handler** 
extensions to run inside Azure Functions with minimal boilerplate, full DI support, structured error 
handling, and complete testability.

---

## ✨ What This Package Provides

### ✔ Full routing pipeline (recommended)

The primary hosting model is the **router function**, powered by `EntraEventRouterFunctionBase`.
It provides:

- Automatic request deserialization  
- Centralized event orchestration  
- Dynamic handler resolution  
- Handler invocation  
- Response serialization  
- Structured error mapping (400/500)  
- Logging for expected and unexpected exceptions  

This allows a **single Azure Function** to host **multiple Entra event types** cleanly.

---

## 🧩 Minimal Router Function

```csharp
public sealed class EntraEventRouterFunction(
    ILogger<EntraEventRouterFunction> logger,
    IEntraEventOrchestrator orchestrator,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraEventRouterFunctionBase(logger, orchestrator, requestAdapter, responseAdapter)
{
    [Function("Router")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "router")]
        HttpRequestData req) =>
        InvokeAsync(req);
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
- Event orchestrator
- Handler resolver
- All handlers implementing `IEntraEventHandler<,>`

---

## 🧠 Handler Resolution

Handlers are resolved dynamically by the orchestrator using the typed resolver:

```csharp
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler<TEvent, TResponse> Resolve<TEvent, TResponse>()
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse;
}
```

The orchestrator selects the correct handler based on the incoming event type and response contract, enabling multi‑event hosting behind a single Azure Function.

---

## 📦 Optional: Single‑Event Base Classes

If you prefer one function per event type:

```csharp
public sealed class TokenIssuanceStartFunction(
    ILogger<TokenIssuanceStartFunction> logger,
    ITokenIssuanceStartHandler handler,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : TokenIssuanceStartFunctionBase(logger, handler, requestAdapter, responseAdapter)
{
    [Function("TokenIssuanceStart")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tokenissuancestart")]
        HttpRequestData req) =>
        InvokeAsync(req);
}
```

---

## 🧪 Testing

The router, adapters, and orchestrator are fully testable thanks to the abstractions.
Unit tests are available in:

Unit tests are available in the  
[Entra.EventHandlers.AzureFunctions.UnitTests](../Entra.EventHandlers.AzureFunctions.UnitTests) project.

---

## 📁 Samples

A complete Azure Functionse sample project is available in the repository under:
[AzureFunctionsSample](../../samples/AzureFunctionsSample) project.

The sample demonstrates:

- Registering handlers with `AddEntraEventHandlers()`
- Using the router function (`EntraEventRouterFunctionBase`)
- Using single‑event function bases
- Exposing functions with `[Function]` and `[HttpTrigger]`
- Structuring a clean, production‑ready Function App

This is the recommended starting point for building real Entra Event Handler extensions on Azure Functions.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)
- **Entra.EventHandlers** — core implementation layer for External ID (BSL)
- **Entra.EventHandlers.Workforce** — Workforce‑specific event models and builders (BSL)
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

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID and Workforce Authentication Event Handlers 
and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
