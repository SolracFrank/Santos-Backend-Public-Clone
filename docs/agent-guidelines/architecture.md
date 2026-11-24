# Architecture

| Project | Responsibility |
| --- | --- |
| `API-main` | Versioned HTTP API, Swagger in development, and application startup. |
| `Application` | Feature handlers, validation, and business rules. |
| `Domain` | Entities, shared models, and contracts. |
| `Infrastructure` | EF Core mappings, MySQL, JWT sessions, audit storage, and mail. |

The database schema is managed outside this repository. Application handlers currently use the infrastructure database context. Keep new HTTP concerns in the API and new business rules in the relevant feature.

The API validates JWTs against the current user's status, role, and security stamp. Read models for attachments omit storage paths. Payment allocation changes run in transactions and validate the customer and available amount.
