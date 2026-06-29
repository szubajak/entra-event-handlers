# Entra.EventHandlers.AspNetCore

**ASP.NET Core hosting adapter for Microsoft Entra External ID and Workforce Authentication Event Handlers.**
Provides minimal‑boilerplate hosting, full DI support, unified exception handling, structured logging, and complete testability.

**License:** Business Source License (BSL)
**Author:** Jakub Szubarga (Szubarga.NET)

---

## ✨ Features

- 🚀 Single endpoint can host **multiple Entra event types**
- 🔄 Automatic request deserialization & response serialization
- 🧠 Centralized event orchestration (routing, resolution, invocation)
- 🧩 Dynamic handler resolution via the orchestrator
- 🛡 Structured error mapping (400/500)
- 🧪 Fully unit‑testable
- 🪶 Minimal boilerplate
- 🧭 Clean endpoint mapping extensions

---

## 🚀 Quick Start

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraEventHandlers();

var app = builder.Build();

app.MapEntraRouter(); // Multi‑event router (recommended)

// Or map individual event endpoints
// app.MapEntraAttributeCollectionStart();
// app.MapEntraAttributeCollectionSubmit();
// app.MapEntraTokenIssuanceStart();
// app.MapEntraEmailOtpSend();
// app.MapPasswordSubmit();
// app.MapVerifiedIdClaimValidation();

app.Run();
```

---

## 🧭 Endpoint Mapping Extensions

```csharp
app.MapEntraRouter();                // Multi‑event router
app.MapEntraTokenIssuanceStart();    // Single‑event endpoint
```

### Default Routes

| Endpoint                    | Default Route                |
|-----------------------------|------------------------------|
| Router                      | `router`                     |
| AttributeCollectionStart    | `attributecollectionstart`   |
| AttributeCollectionSubmit   | `attributecollectionsubmit`  |
| TokenIssuanceStart          | `tokenissuancestart`         |
| EmailOtpSend                | `emailotpsend`               |
| PasswordSubmit              | `passwordsubmit`             |
| VerifiedIdClaimValidation   | `verifiedidclaimvalidation`  |

---

## 🛠 Dependency Injection

```csharp
services.AddEntraEventHandlers();
```

Registers:

- Request/response adapters
- Event orchestrator
- Handler resolver
- All `IEntraEventHandler<,>` implementations
- All ASP.NET Core endpoint classes (router + single‑event)

---

## 🔧 Extensibility

All endpoints inherit from a unified execution pipeline with:

- Overridable logging hooks
- Centralized exception handling
- Consistent request/response processing

---

## 📁 Samples

👉 **ApiSample**
https://github.com/szubajak/entra-event-handlers/tree/main/samples/ApiSample

The sample demonstrates:

- registering handlers with `AddEntraEventHandlers()`
- mapping the router endpoint
- mapping individual single‑event endpoints
- using the unified execution pipeline
- structuring a clean, minimal ASP.NET Core API for Entra event handling

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

- `LICENSE` — full BSL terms
- `LICENSE-COMMERCIAL.md` — commercial licensing terms

A commercial license is required for production use by organizations with more than 5 employees.

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current and future BSL‑licensed packages.

### Commercial License Pricing

- **Developer License** — €99 / developer / year
- **Team License** — €399 / year
- **Enterprise License** — €1499 / year

For commercial licensing or support:
📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID Authentication Event Handlers,
Workforce scenarios, and the design of this ecosystem, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437
