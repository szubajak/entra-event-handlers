# Entra.EventHandlers.AzureFunctions

Azure Functions hosting adapter for Microsoft Entra External ID Authentication Event Handlers.  
This package builds on top of **Entra.EventHandlers** and **Entra.EventHandlers.Abstractions** to provide automatic request/response handling and minimal‑boilerplate function implementations.

---

## 🚀 Features

### ✔ Automatic Request Deserialization

```csharp
var evt = await HttpRequestAdapter.ReadEvent<TokenIssuanceStartEvent>(req);
```

### ✔ Automatic Response Serialization

```csharp
return await HttpResponseAdapter.From(req, response);
```

### ✔ Minimal Function Base Classes

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

Derived function:

```csharp
public class TokenFunction : TokenIssuanceStartFunctionBase
{
    public TokenFunction(ITokenIssuanceStartHandler handler) : base(handler) {}

    [Function("TokenIssuanceStart")]
    public Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)
        => base.Run(req, ctx);
}
```

### ✔ DI‑Friendly Architecture

Handlers are resolved via dependency injection, enabling clean separation of:

- Hosting  
- Business logic  
- Response construction  

---

## 🛠 Example: Full Function + Handler

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

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — protocol types (MIT)  
- **Entra.EventHandlers** — implementation layer (BSL)  

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

See:  
- LICENSE  
- LICENSE-COMMERCIAL.md  

A commercial license is required for production use by organizations with more than 5 employees.

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current and future BSL‑licensed packages.

### Commercial License Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

For commercial licensing or support:  
📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.
