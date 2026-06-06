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
public sealed class EntraRouterFunction : EntraEventRouterFunctionBase
{
    public EntraRouterFunction(
        ILogger<EntraEventRouterFunctionBase> logger,
        IEntraEventHandlerResolver resolver,
        IRequestAdapter requestAdapter,
        IResponseAdapter responseAdapter)
        : base(logger, resolver, requestAdapter, responseAdapter) {}

    [Function("EntraRouter")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)
        => Run(req, ctx);
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
public sealed class TokenIssuanceStartFunction : TokenIssuanceStartFunctionBase
{
    public TokenIssuanceStartFunction(
        ITokenIssuanceStartHandler handler,
        IRequestAdapter requestAdapter,
        IResponseAdapter responseAdapter)
        : base(handler, requestAdapter, responseAdapter) {}

    [Function("TokenIssuanceStart")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)
        => Run(req, ctx);
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
