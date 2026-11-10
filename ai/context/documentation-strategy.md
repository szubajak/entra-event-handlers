# Documentation Strategy

Documentation exists for both humans and AI assistants.

Priority documentation sources:

1. Root README.md
2. docs/architecture.md
3. docs/getting-started.md
4. Package README files
5. Events documentation
6. Hosting documentation

All public-facing changes should evaluate whether documentation updates are required.

Evaluate documentation by change type:

| Change | Documentation to review |
|---|---|
| Public abstraction or protocol | Root README, package README, `CHANGELOG.md`, architecture documentation |
| New event | `docs/events/*` and README discoverability |
| Hosting behavior | `docs/hosting/*`, getting started, and hosting package README |
| Security behavior | Security package README and relevant security documentation |
| Observability behavior | Observability package README and architecture documentation |
| Test or developer workflow | `docs/testing.md` and relevant AI context |
| Architecture or dependency change | `docs/architecture.md` and relevant AI context |

Documentation should:
- use descriptive headings
- include examples
- explain relationships between concepts
- be understandable by both humans and AI systems