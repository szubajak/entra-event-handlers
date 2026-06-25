# Entra.EventHandlers.AzureFunctions

**Azure Functions hosting adapter for Microsoft Entra External ID Authentication Event Handlers.**  
Provides minimal‑boilerplate hosting, full DI support, structured error handling, and complete testability.

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

---

## ✨ Features

- 🚀 **Single Function → Multiple Entra Event Types**  
- 🔄 **Automatic request deserialization & response serialization**  
- 🧩 **Dynamic handler resolution via DI**  
- 🛡 **Structured error mapping (400/500)**  
- 🧪 **Fully unit‑testable**  
- 🪶 **Minimal boilerplate**

---

## 🧩 Minimal Router Function

```csharp
public sealed class EntraEventRouterFunction(
    ILogger<EntraEventRouterFunction> logger,
    IEntraEventHandlerResolver resolver,
    IRequestAdapter requestAdapter,
    IResponseAdapter responseAdapter)
    : EntraEventRouterFunctionBase(logger, resolver, requestAdapter, responseAdapter)
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
- Handler resolver
- All handlers implementing `IEntraEventHandler<,>`

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

## 📁 Samples

A complete Azure Functions sample project is available in the repository:

👉 **AzureFunctionsSample**  
https://github.com/szubajak/entra-event-handlers/tree/main/samples/AzureFunctionsSample

The sample demonstrates:

- registering handlers with `AddEntraEventHandlers()`
- using the router function (`EntraEventRouterFunctionBase`)
- using single‑event function bases
- exposing functions with `[Function]` and `[HttpTrigger]`
- structuring a clean, minimal Function App for Entra event handling

This is the recommended starting point for building real Entra Event Handler extensions on Azure Functions.

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

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID Authentication Event Handlers
and the design of this ecosystem, see the full article:

➡️ **Entra External ID — .NET Handlers Deep Dive**  
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
