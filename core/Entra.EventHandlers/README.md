# Entra.EventHandlers

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga — Szubarga.NET

This package contains the full implementation layer for the Entra Event
Handlers ecosystem. It builds on top of the MIT‑licensed
**Entra.EventHandlers.Abstractions** package and provides higher‑level
functionality for constructing responses, composing handlers, and building
production‑ready authentication event extensions.

---

## ✨ What This Package Provides

This package extends the abstractions with implementation features such as:

- Fluent **response builders** for all Entra event types  
  - `AttributeCollectionStartResponseBuilder`  
  - `AttributeCollectionSubmitResponseBuilder`  
  - `TokenIssuanceStartResponseBuilder`

- A unified entry point:  
  - `EntraEventResponses.AttributeCollectionStart()`  
  - `EntraEventResponses.AttributeCollectionSubmit()`  
  - `EntraEventResponses.TokenIssuanceStart()`

These builders provide a strongly‑typed, discoverable, and ergonomic way to
construct valid Entra responses without manually crafting JSON payloads.

More features will be added over time, including:

- Validation and error handling helpers  
- Execution pipeline components  
- Routing and handler composition  
- Logging and telemetry hooks  
- Test utilities  
- Integration helpers for Azure Functions and other hosts  

---

## 🧩 Relationship to the Abstractions Package

This package depends on:

- **Entra.EventHandlers.Abstractions** (MIT)

The abstractions define the protocol and public contract.  
This package provides the implementation and developer experience on top of it.

You are free to use the abstractions in any project (open‑source or commercial).  
This implementation package is licensed under the **Business Source License (BSL)**.

---

## 📦 Related Packages

- **Entra.EventHandlers.Abstractions** — public protocol types (MIT)  
- **Entra.EventHandlers.AzureFunctions** — Azure Functions integration (BSL)  
  - Automatic request/response handling  
  - DI wiring  
  - Minimal boilerplate for production deployments  

These packages will be published to NuGet soon.

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.  
See the [LICENSE](./LICENSE) file for details.

For commercial production use, enterprise licensing, or support inquiries:
📧 jakub.szubarga@gmail.com

The abstractions package is MIT‑licensed and can be used freely.

---

## 📚 Documentation

Full documentation, examples, and production templates will be available in the
main repository as the ecosystem evolves.
