[![Sponsor](https://img.shields.io/badge/Sponsor-%E2%9D%A4-red)](https://github.com/sponsors/szubajak)

## Packages

### Abstractions
[![NuGet Abstractions](https://img.shields.io/nuget/v/Entra.EventHandlers.Abstractions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Abstractions)
[![License: MIT (Abstractions)](https://img.shields.io/badge/License-MIT-blue.svg)](abstractions/Entra.EventHandlers.Abstractions/LICENSE)

### Core
[![NuGet Core](https://img.shields.io/nuget/v/Entra.EventHandlers.svg)](https://www.nuget.org/packages/Entra.EventHandlers)
[![License: BSL (Core)](https://img.shields.io/badge/License-BSL-orange.svg)](core/Entra.EventHandlers/LICENSE)

### AzureFunctions
[![NuGet Core](https://img.shields.io/nuget/v/Entra.EventHandlers.AzureFunctions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AzureFunctions)
[![License: BSL (AzureFunctions)](https://img.shields.io/badge/License-BSL-orange.svg)](functions/Entra.EventHandlers.AzureFunctions/LICENSE)


# Entra Event Handlers — .NET Ecosystem

A modern, strongly‑typed, developer‑friendly ecosystem for building  
**Microsoft Entra ID Authentication Event Handlers** in .NET.

This solution provides:

- MIT‑licensed **public abstractions**
- BSL‑licensed **full implementation**
- BSL‑licensed **Azure Functions integration**
- Fluent response builders
- Protocol‑accurate request/response models
- Base handler infrastructure (logging, validation, timing, correlation)
- A clean, extensible architecture designed for production workloads

---

## 📦 Projects in This Repository

### **1. Entra.EventHandlers.Abstractions** (MIT)

Lightweight, dependency‑free abstractions defining the public contract:

- Event request models  
- Response models and action types  
- Event and OData constants  
- Directory attribute primitives  
- Interfaces for building custom handlers  

This package is safe to reference in any open‑source or commercial project.

➡️ *See the package README for details.*

---

### **2. Entra.EventHandlers** (BSL)

The full implementation layer built on top of the abstractions:

- Fluent response builders  
- Strongly‑typed construction of Entra responses  
- `PrefillValuesBuilder` for attribute prefill scenarios  
- Unified entry point (`EntraEventResponses.*`)  
- Base handler infrastructure:
  - Structured logging  
  - Correlation scoping  
  - Execution timing  
  - Protocol‑level validation (`@odata.type`)  
  - Consistent exception handling  
  - Clean override point (`HandleCore`)  

This package is licensed under the **Business Source License (BSL)**  
and becomes MIT after the Change Date.

➡️ *See the package README for details.*

---

### **3. Entra.EventHandlers.AzureFunctions** (BSL)

Azure Functions integration for production deployments:

- Automatic request parsing  
- Automatic response serialization  
- DI wiring  
- Minimal boilerplate  
- Future: middleware, logging, correlation, metrics  

This package is also licensed under the **Business Source License (BSL)**.

➡️ *See the package README for details.*

---

## 🧩 Architecture Overview

The ecosystem is intentionally split into layers:

```
┌──────────────────────────────────────────────┐
│ Entra.EventHandlers.Abstractions (MIT)       │
│ Public contract, protocol types, interfaces  │
└──────────────────────────────────────────────┘
                 ▲
                 │
┌──────────────────────────────────────────────┐
│ Entra.EventHandlers (BSL)                    │
│ Implementation, builders, pipelines          │
│ Base handlers, validation, logging           │
└──────────────────────────────────────────────┘
                 ▲
                 │
┌──────────────────────────────────────────────┐
│ Entra.EventHandlers.AzureFunctions (BSL)     │
│ Hosting, DI, runtime integration             │
└──────────────────────────────────────────────┘
```

This separation ensures:

- **Maximum adoption** (MIT abstractions)  
- **Commercial protection** (BSL implementation)  
- **Clean extensibility**  
- **Stable public API surface**  

---

## 🛠 Example: Building a Response

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .ShowBlockPage("Error", "Unexpected error occurred.")
    .Build();
```

With prefill:

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .PrefillValues(p => p
        .With("email", "user@example.com")
        .With("country", "PL"))
    .Build();
```

---

## 🛠 Example: Implementing a Handler

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
```

The base class automatically provides:

- CorrelationId logging  
- EventType/EventName scoping  
- Duration measurement  
- Validation  
- Exception handling  

---

## 🔒 Licensing Model

This repository uses a hybrid licensing approach:

- **MIT** for the abstractions  
- **BSL** for the implementation and hosting layers  

The BSL packages:

- allow free non‑production use  
- allow limited production use (small teams)  
- automatically convert to MIT after the Change Date  

This model keeps the ecosystem open while supporting sustainable development.

---

## 💼 Commercial Licensing

The **Entra.EventHandlers (Core)** and **Entra.EventHandlers.AzureFunctions** packages are licensed under the Business Source License (BSL).

A commercial license covers the entire Entra Event Handlers ecosystem,
including all current and future BSL‑licensed packages.

Commercial licenses are available:

- Developer License — €99 / developer / year  
- Team License — €399 / year  
- Enterprise License — €1499 / year  

To purchase a license, contact:

📧 jakub.szubarga@gmail.com

➡️ See [LICENSE-COMMERCIAL.md](LICENSE-COMMERCIAL.md) for full commercial terms.

---

## 🚀 Roadmap

Upcoming features include:

- Validation and error‑handling helpers  
- Execution pipeline and middleware  
- Handler routing  
- Logging and telemetry hooks  
- Test utilities  
- Full documentation site  
- Production templates for Azure Functions  
- Sample implementations and scenarios  

---

## 📚 Documentation

Documentation, examples, and production templates will be published in the main
repository as the ecosystem evolves.

---

## 🤝 Contributing

Contributions to the MIT abstractions package are welcome.  
Implementation packages follow a controlled contribution model due to BSL.

---

## ❤️ Support the project

If you find this library useful, consider sponsoring development:

👉 https://github.com/sponsors/szubajak

---

## 🧑‍💻 Author

**Jakub Szubarga (Szubarga.NET)**


If you find this ecosystem useful, consider starring the repository ⭐
