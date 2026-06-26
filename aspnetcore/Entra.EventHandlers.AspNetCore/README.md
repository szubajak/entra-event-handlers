# Entra.EventHandlers.AspNetCore

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package provides the **ASP.NET Core hosting adapter** for the Entra Event Handlers ecosystem.  
It enables production‑ready **Microsoft Entra External ID and Workforce Authentication Event Handler**  
extensions to run inside ASP.NET Core with:

- Minimal boilerplate  
- Full DI support  
- Unified exception handling  
- Structured logging  
- Clean endpoint mapping  
- Complete testability  

---

## ✨ What This Package Provides

### ✔ Unified hosting pipeline  
A consistent execution model for all Entra event handlers:

- Request deserialization  
- Event orchestration (resolution → invocation)  
- Response serialization  
- Structured error mapping  
- Logging for known and unknown exceptions  

### ✔ Router endpoint (multi‑event hosting)  
A single endpoint capable of hosting **multiple Entra event types**.

### ✔ Single‑event endpoints  
Explicit endpoints for scenarios where you want separate routes per event.

### ✔ Endpoint mapping extensions  
Consumers map endpoints using simple, explicit extension methods.

---

## 🚀 Quick Start

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraEventHandlers();

var app = builder.Build();

// Option A: Multi‑event router
app.MapEntraRouter();

// Option B: Individual event endpoints
// app.MapEntraAttributeCollectionStart();
// app.MapEntraAttributeCollectionSubmit();
// app.MapEntraTokenIssuanceStart();
// app.MapEntraEmailOtpSend();
// app.MapPasswordSubmit();

app.Run();
```

---

## 🧭 Endpoint Mapping Extensions

This package exposes extension methods for clean, explicit endpoint registration:

```csharp
app.MapEntraRouter();                // Multi‑event router
app.MapEntraTokenIssuanceStart();    // Single‑event endpoint
```

These extensions:

- Resolve the endpoint class from DI  
- Call its `Map()` method  
- Attach the correct route  
- Ensure unified exception handling and logging  

### Default Routes

| Endpoint                    | Default Route                |
|-----------------------------|------------------------------|
| Router                      | `router`                     |
| AttributeCollectionStart    | `attributecollectionstart`   |
| AttributeCollectionSubmit   | `attributecollectionsubmit`  |
| TokenIssuanceStart          | `tokenissuancestart`         |
| EmailOtpSend                | `emailotpsend`               |
| PasswordSubmit              | `passwordsubmit`             |

---

## 🧩 Router Endpoint (Recommended)

The router endpoint (`EntraEventRouterEndpoint`) provides:

- Automatic event deserialization  
- Automatic orchestration of the event execution pipeline  
- Automatic handler resolution (via the orchestrator)  
- Automatic handler invocation  
- Automatic response serialization  
- Structured error responses  
- Logging for expected and unexpected exceptions  

### Mapping the router

```csharp
app.MapEntraRouter();
```

This exposes a POST endpoint (default `/router`) that can host multiple event types behind a single route.

---

## 📦 Single‑Event Endpoints

If you prefer explicit per‑event routes, you can map individual endpoints:

```csharp
app.MapEntraTokenIssuanceStart();
```

This exposes a POST endpoint (default `/tokenissuancestart`) that:

- Deserializes the event  
- Invokes the correct handler  
- Writes the response  
- Logs exceptions  
- Uses the same unified pipeline as the router  

All single‑event endpoint classes are included in this package.

---

## 🛠 Dependency Injection

Register all required components with:

```csharp
services.AddEntraEventHandlers();
```

This automatically registers:

- Request/response adapters  
- Event orchestrator  
- Handler resolver  
- All handlers implementing `IEntraEventHandler<,>`  
- All ASP.NET Core endpoint classes (router + single‑event endpoints)  

Endpoints are activated automatically when you map them:

```csharp
app.MapEntraRouter();
app.MapEntraTokenIssuanceStart();
```

---

## 🧠 Handler Resolution

Handlers are resolved dynamically by the event orchestrator, which uses the typed resolver:

```csharp
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler<TEvent, TResponse> Resolve<TEvent, TResponse>()
        where TEvent : EntraEvent
        where TResponse : EntraEventResponse;
}
```

This enables multi‑event hosting behind a single ASP.NET Core endpoint.

---

## 🔧 Extensibility

All endpoints inherit from a unified execution pipeline with:

- Overridable logging hooks  
- Centralized exception handling  
- Consistent request/response processing  

This allows advanced consumers to customize behavior while keeping the core pipeline intact.

---

## 🧪 Testing

The router, adapters, and orchestrator are fully testable thanks to the abstractions.

Unit tests are available in:  
👉 [Entra.EventHandlers.AspNetCore.UnitTests](../Entra.EventHandlers.AspNetCore.UnitTests)

Integration tests are available in:  
👉 [Entra.EventHandlers.AspNetCore.IntegrationTests](../Entra.EventHandlers.AspNetCore.IntegrationTests)

---

## 📁 Samples

A complete ASP.NET Core sample project is available in the repository under:  
[ApiSample](../../samples/ApiSample) project.

The sample demonstrates:

- Registering handlers with `AddEntraEventHandlers()`  
- Mapping the router endpoint (`app.MapEntraRouter()`)  
- Mapping individual single‑event endpoints  
- Using the unified execution pipeline (deserialization → orchestration → resolution → invocation → response)  
- Structuring a clean, production‑ready ASP.NET Core application  

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers** — core implementation layer for External ID (BSL)  
- **Entra.EventHandlers.Workforce** — Workforce‑specific event models and builders (BSL)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions hosting adapter (BSL)

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
