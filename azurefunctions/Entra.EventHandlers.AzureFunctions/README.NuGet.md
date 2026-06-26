# Entra.EventHandlers.AzureFunctions

**Azure Functions hosting adapter for Microsoft Entra External ID and Workforce Authentication Event Handlers.**  
Provides minimal‑boilerplate hosting, full DI support, structured error handling, and complete testability.

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

---

## ✨ Features

- 🚀 **Single Function → Multiple Entra event types**  
- 🔄 Automatic request deserialization & response serialization  
- 🧠 Centralized event orchestration (routing, resolution, invocation)  
- 🧩 Dynamic handler resolution via the orchestrator  
- 🛡 Structured error mapping (400/500)  
- 🧪 Fully unit‑testable  
- 🪶 Minimal boilerplate

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

```csharp
services.AddEntraEventHandlers();
```

Registers:

- Request/response adapters  
- Event orchestrator  
- Handler resolver  
- All handlers implementing `IEntraEventHandler<,>`

---

## 📦 Optional: Single‑Event Base Classes

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

## 📁 Samples

👉 **AzureFunctionsSample**  
https://github.com/szubajak/entra-event-handlers/tree/main/samples/AzureFunctionsSample

The sample demonstrates:

- registering handlers with `AddEntraEventHandlers()`  
- using the router function  
- using single‑event function bases  
- exposing functions with `[Function]` and `[HttpTrigger]`  
- structuring a clean, minimal Function App for Entra event handling  

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
- `LICENSE` — full BSL terms  
- `LICENSE-COMMERCIAL.md` — commercial licensing terms  

A commercial license is required for production use by organizations with more than 5 employees.

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current and future BSL‑licensed packages.

### Commercial License Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID Authentication Event Handlers,
Workforce scenarios, and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
