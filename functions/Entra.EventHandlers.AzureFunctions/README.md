# Entra.EventHandlers.AzureFunctions

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package provides the **Azure Functions hosting adapter** for the Entra Event Handlers ecosystem. It enables production‑ready **Microsoft Entra External ID Authentication Event Handler** extensions to run inside Azure Functions with minimal boilerplate, full DI support, structured error handling, and complete testability.

---

## ✨ What This Package Provides

### ✔ Full routing pipeline

The recommended hosting model is the **router function**, powered by `EntraEventRouterFunctionBase`. It provides:

- Automatic request deserialization  
- Automatic handler resolution  
- Automatic handler invocation  
- Automatic response serialization  
- Structured error mapping  
- Logging for expected and unexpected exceptions  

This allows a **single Azure Function** to host **multiple Entra event types** cleanly.

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

## 🧠 Handler Resolution

Handlers are resolved dynamically based on the event type:

```csharp
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler Resolve(Type eventType);
}
```

This enables multi‑event hosting behind a single Azure Function.

---

## 📦 Optional: Single‑Event Base Classes

You can use the simple single‑event hosting model:

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

## 🧪 Testing

The router and adapters are fully testable thanks to the abstractions.  
Unit tests are available in the  
[Entra.EventHandlers.AzureFunctions.UnitTests](../Entra.EventHandlers.AzureFunctions.UnitTests) project.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers** — core implementation layer (BSL)
- **Entra.EventHandlers.AspNetCore** — ASP.NET Core hosting adapter (BSL)  

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
