# Entra.EventHandlers.AspNetCore

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package provides the **ASP.NET Core hosting adapter** for the Entra Event Handlers ecosystem.  
It enables production‑ready **Microsoft Entra External ID Authentication Event Handler** extensions to run inside ASP.NET Core with:

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
- Handler resolution  
- Handler invocation  
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

---

## 🧩 Router Endpoint (Recommended)

The router endpoint (EntraEventRouterEndpoint) provides:
- Automatic event deserialization
- Dynamic handler resolution
- Automatic invocation
- Structured error responses
- Logging for expected and unexpected exceptions

### Mapping the router

```csharp
app.MapEntraRouter();
```

This exposes a POST endpoint (default /entra/router) that can host multiple event types behind a single route.

---

## 📦 Single‑Event Endpoints

If you prefer explicit per‑event routes, you can map individual endpoints:

```csharp
app.MapEntraTokenIssuanceStart();
```

This exposes a POST endpoint (default /entra/tokenissuancestart) that:
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

Handlers are resolved dynamically based on the event type:

```csharp
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler Resolve(Type eventType);
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

The router and adapters are fully testable thanks to the abstractions.  
Unit tests are available in the  
[Entra.EventHandlers.AspNetCore.UnitTests](../Entra.EventHandlers.AspNetCore.UnitTests) project.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers** — core implementation layer (BSL)
- **Entra.EventHandlers.AzureFunctions** — Azure Functions hosting adapter (BSL)

---

## 📁 Samples

This package includes a full ASP.NET Core sample demonstrating how to host Entra Event Handlers in a real Web API application:

- **ApiSample** — minimal ASP.NET Core API using  
  `EntraEventRouterEndpoint` and single‑event endpoint classes.

The sample shows:

- How to register handlers with `AddEntraEventHandlers()`
- How to map the router endpoint using `app.MapEntraRouter()`
- How to map individual single‑event endpoints
- How the unified execution pipeline handles deserialization, resolution, invocation, and response writing
- How to structure a clean, production‑ready ASP.NET Core application for Entra event handling

You can find the sample in the repository under:  
[ApiSample](../samples/ApiSample) project.

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
