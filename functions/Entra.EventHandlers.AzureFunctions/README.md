# Entra.EventHandlers.AzureFunctions

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package provides the **Azure Functions hosting adapter** for the Entra Event Handlers ecosystem.  
It builds on top of:

- **Entra.EventHandlers.Abstractions** (MIT)  
- **Entra.EventHandlers** (BSL)

and enables production‑ready **Microsoft Entra External ID Authentication Event Handler** extensions to run inside Azure Functions with minimal boilerplate.

---

## ✨ What This Package Provides

### ✔ Automatic request deserialization

Incoming HTTP requests are automatically converted into strongly‑typed event objects:

```csharp
var evt = await HttpRequestAdapter.ReadEvent<TokenIssuanceStartEvent>(req);
```

### ✔ Automatic response serialization

Handler responses are automatically converted into valid HTTP responses:

```csharp
return await HttpResponseAdapter.From(req, response);
```

### ✔ Minimal Function base classes

Each event type has a dedicated base class that handles:

- Request parsing  
- Handler invocation  
- Response serialization  

Example:

```csharp
public abstract class TokenIssuanceStartFunctionBase
{
    private readonly ITokenIssuanceStartHandler _handler;

    protected TokenIssuanceStartFunctionBase(ITokenIssuanceStartHandler handler)
    {
        _handler = handler;
    }

    public async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await HttpRequestAdapter.ReadEvent<TokenIssuanceStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await HttpResponseAdapter.From(req, response);
    }
}
```

Derived functions only need to wire the trigger:

```csharp
public class MyTokenFunction : TokenIssuanceStartFunctionBase
{
    public MyTokenFunction(ITokenIssuanceStartHandler handler)
        : base(handler) {}

    [Function("TokenIssuanceStart")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)
        => base.Run(req, ctx);
}
```

### ✔ DI‑friendly design

Handlers are resolved via dependency injection, enabling clean separation of:

- Hosting  
- Business logic  
- Response construction  

---

## 🧩 Relationship to Other Packages

This package depends on:

- **Entra.EventHandlers.Abstractions** (MIT)  
- **Entra.EventHandlers** (BSL)

Use this package when hosting your Entra event handlers in **Azure Functions**.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers** — implementation layer (BSL)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions integration (BSL) ← this package  

---

## 🛠 Example: Full Azure Function

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

public class AttributeCollectionStartFunction : AttributeCollectionStartFunctionBase
{
    public AttributeCollectionStartFunction(IAttributeCollectionStartHandler handler)
        : base(handler) {}

    [Function("AttributeCollectionStart")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)
        => base.Run(req, ctx);
}
```

This gives you:

- Automatic deserialization  
- Automatic serialization  
- Logging, validation, timing (from handler base class)  
- Minimal boilerplate  

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

See:

- **LICENSE** — full BSL terms  
- **LICENSE-COMMERCIAL.md** — commercial licensing terms  

A commercial license is required for production use by organizations with more than 5 employees.

A commercial license covers the entire **Entra Event Handlers ecosystem**, including all current and future BSL‑licensed packages.

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
