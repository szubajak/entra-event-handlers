# Changelog

All notable changes to this project's [**Entra.EventHandlers.Abstractions**][nuget] package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project attempts to adhere to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## v1.0.0 — 2026‑05‑28

### Added

#### Core Event Models
- Added `AttributeCollectionStartEvent` request model.
- Added `AttributeCollectionSubmitEvent` request model.
- Added `TokenIssuanceStartEvent` request model.
- Added shared base types for event metadata and protocol primitives.

#### Response Models
- Added `AttributeCollectionStartResponse`.
- Added `AttributeCollectionSubmitResponse`.
- Added `TokenIssuanceStartResponse`.
- Added base `EntraEventResponse` type.

#### Action Types
- Added `ContinueAction` with `ContinueActionType`.
- Added `ShowBlockPageAction` with `ShowBlockPageActionType`.
- Added `SetPrefillValuesAction`.
- Added `ShowValidationErrorAction`.
- Added `ModifyAttributeValuesAction` (if included in abstractions).
- Added base `EntraAction` type.

#### Protocol Constants
- Added `EntraEventTypes` for event identifiers.
- Added `EntraOdataTypes` for all OData discriminators:
  - `AttributeCollectionStart`
  - `AttributeCollectionSubmit`
  - `TokenIssuanceStart`
  - `DirectoryAttributes`
- Added `DirectoryAttributeTypes` for attribute classification.

#### Directory Attribute Models
- Added strongly‑typed directory attribute value models:
  - `StringDirectoryAttributeValue`
  - `Int64DirectoryAttributeValue`
  - `BooleanDirectoryAttributeValue`
- Added base `DirectoryAttributeValue`.

#### Metadata & Enums
- Added enums and helper types for:
  - Attribute metadata
  - Action discriminators
  - Event type mapping

#### Documentation
- Added full XML documentation for all public types.
- Added MIT license.
- Added initial README for the abstractions package.

---