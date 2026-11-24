# Santos Backend

REST API for customers, estimates, invoices, and payments. Built with ASP.NET Core, EF Core, MySQL, and JWT authentication.

## Build and test

Requires the .NET 10 SDK.

```sh
dotnet build API-main/EsApi.csproj
dotnet test Tests/EsApi.Tests/EsApi.Tests.csproj
```

The test suite runs the API against an in-memory database. Use MySQL to verify transactions and concurrency.

## Run locally

Use an existing MySQL database with the application schema. Store development secrets in .NET User Secrets and production secrets in your deployment's secret store.

| Environment variable | Purpose |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | MySQL connection string. |
| `Jwt__Issuer`, `Jwt__Audience`, `Jwt__SecretKey` | JWT settings. Use a signing key generated from at least 32 random bytes. |
| `API_BOOTSTRAP_ADMIN_PASSWORD` | Optional password for creating the first administrator when the user table is empty. Remove it after setup. |
| `MicrosoftGraphEmail__TenantId`, `MicrosoftGraphEmail__ClientId`, `MicrosoftGraphEmail__ClientSecret`, `MicrosoftGraphEmail__FromEmail` | Optional Microsoft Graph mail settings. |
| `Sentry__Dsn` | Optional error reporting. |

```sh
dotnet run --project API-main/EsApi.csproj
```

Swagger is available in development. Call `POST /api/v1/auth/login` for a JWT and send it as `Authorization: Bearer <token>`. Attachment operations and writes to invoices and payments require the `Administrator` role.

See the [architecture notes](docs/agent-guidelines/architecture.md) and [release checklist](docs/release-checklist.md).

## License

All rights reserved. See [LICENSE](LICENSE).
