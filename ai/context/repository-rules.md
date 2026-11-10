# Repository Rules

## Documentation

All user-facing functionality must be documented.

When introducing a feature:

- Update README.md when discoverability is affected.
- Update CHANGELOG.md if Entra.EventHandlers.Abstractions is affected
- Update architecture.md when architecture changes.
- Update getting-started.md when setup changes.
- Update hosting documentation when hosting behavior changes.
- Update event documentation when event behavior changes.

## AI Discoverability

Documentation should be understandable by:

- Human developers
- AI assistants

Prefer:

- Clear headings
- Descriptive section names
- Concrete examples
- Explicit relationships between concepts

Avoid:

- Hidden assumptions
- Unexplained abbreviations
- Fragmented documentation

## Pull Requests

Feature pull requests should contain:

- Implementation
- Tests
- Documentation updates

A feature is not complete until:

- implementation exists
- tests exist
- documentation exists

Documentation review must include:

- root README.md
- package README.md files
- docs/architecture.md
- docs/getting-started.md
- affected docs/events/*
- affected docs/hosting/*

Documentation should remain:
- human friendly
- AI friendly

## Package Metadata

When modifying a packable project:

- Preserve the existing `PackageId`, license, package README, icon, and repository metadata unless the change explicitly requires an update.
- Do not change package references or versions without explicit justification.
- Evaluate package versioning for breaking or externally visible changes.
- Update `CHANGELOG.md` for public Abstractions changes.

## Validation

After code changes, run the relevant restore, build, and test validation. Prefer targeted tests during iteration and a full solution build before completion.
