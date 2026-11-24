# Operations API

API REST para gestionar clientes, presupuestos, facturas, pagos y registros operativos. Usa ASP.NET Core, EF Core con MySQL, MediatR, FluentValidation y autenticación JWT.

## Estructura

- `API-main/` expone controladores HTTP, Swagger en desarrollo y respuestas Problem Details.
- `Application/` organiza los casos de uso, validadores y autorización por operación.
- `Domain/` define entidades, contratos y errores de negocio.
- `Infrastructure/` implementa persistencia, autenticación, auditoría y correo.
- `Tests/` ejecuta la API en memoria para comprobar permisos, revocación y validaciones. Para comprobar el comportamiento transaccional, usa también MySQL en un entorno de pruebas.

## Compilar y probar

Instala el SDK de .NET 10.

```sh
dotnet build API-main/EsApi.csproj
dotnet test Tests/EsApi.Tests/EsApi.Tests.csproj
```

Para ejecutar la API, usa una base MySQL con el esquema correspondiente. El modelo de EF es database-first. Configura los valores con User Secrets en desarrollo y con el gestor de secretos del entorno en producción.

| Variable de entorno | Uso |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | Cadena de conexión de MySQL. |
| `Jwt__Issuer`, `Jwt__Audience`, `Jwt__SecretKey` | Emisor, audiencia y clave de firma JWT. Usa una clave aleatoria de al menos 32 bytes. |
| `API_BOOTSTRAP_ADMIN_PASSWORD` | Credencial de uso único para aprovisionar el primer administrador cuando la tabla de usuarios está vacía. Retírala tras el primer arranque. |
| `MicrosoftGraphEmail__TenantId`, `MicrosoftGraphEmail__ClientId`, `MicrosoftGraphEmail__ClientSecret`, `MicrosoftGraphEmail__FromEmail` | Configuración opcional del envío de correo. |
| `Sentry__Dsn` | Telemetría opcional. |

Con esas variables configuradas:

```sh
dotnet run --project API-main/EsApi.csproj
```

Autentícate con `POST /api/v1/auth/login` y envía el JWT como encabezado `Authorization: Bearer …`. Las operaciones de adjuntos y las escrituras de facturas y pagos requieren el rol `Administrator`. Las respuestas de adjuntos omiten la ruta de almacenamiento.
