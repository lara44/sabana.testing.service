# sabana.testing.service

Guia de puesta en marcha local del servicio.

## Requisitos

- .NET SDK `10.0.101` (ver `global.json`).
- Docker Desktop (o Docker Engine + Compose).
- Puerto libre `5443` para PostgreSQL.
- Puertos libres `5121` (HTTP) y `7024` (HTTPS) para WebApi.

## Estructura relevante

- `infrastructure/sabana-testing/docker-compose.yaml`: contenedor de PostgreSQL.
- `src/WebApi/appsettings.Development.json`: connection string de desarrollo.
- `src/WebApi/Program.cs`: aplica migraciones automaticamente al iniciar (`Database.MigrateAsync`).

## 1. Levantar base de datos en contenedor

Desde la raiz del repositorio:

```bash
docker compose -f infrastructure/sabana-testing/docker-compose.yaml up -d
```

Verificar estado:

```bash
docker compose -f infrastructure/sabana-testing/docker-compose.yaml ps
```

Para detener:

```bash
docker compose -f infrastructure/sabana-testing/docker-compose.yaml down
```

## 2. Configurar appsettings de desarrollo

Asegura que `src/WebApi/appsettings.Development.json` tenga una cadena valida para el contenedor local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5443;Database=sabana-testing_db;Username=postgres;Password=postgres"
  }
}
```

Notas:

- `appsettings.json` puede dejar `DefaultConnection` vacia para no hardcodear valores globales.
- En entorno `Development`, ASP.NET Core usa `appsettings.Development.json`.

## 3. Ejecutar el servicio

Desde la raiz:

```bash
dotnet run --project src/WebApi/WebApi.csproj
```

Al iniciar:

- Se registran dependencias de `Application`, `Infrastructure` y `Presentation`.
- Se ejecutan migraciones automaticamente contra la base configurada.

URLs locales (segun `launchSettings.json`):

- `http://localhost:5121`
- `https://localhost:7024`

Swagger/OpenAPI en desarrollo:

- `https://localhost:7024/swagger`
- `http://localhost:5121/swagger`

## 4. Verificacion rapida

- Endpoint raiz: `GET /` responde `Hello World!`.
- Endpoint de productos: `GET /api/products`.

## 5. Pruebas de integracion (SQLite InMemory)

Las pruebas de integracion de la Unidad 4 usan SQLite InMemory para ejecutarse sin infraestructura externa.
Esto permite correrlas en local y en CI sin depender de Docker ni de una base fisica.

Ejemplo local:

```bash
dotnet test tests/sabana.testing.service.tests/sabana.testing.service.tests.csproj --filter "FullyQualifiedName~integration"
```

## Problemas comunes

- Error de conexion a BD:
  - Verifica que el contenedor este `Up` y saludable.
  - Confirma puerto `5443` y credenciales en `appsettings.Development.json`.
- Error de puerto en WebApi:
  - Libera `5121`/`7024` o ajusta `launchSettings.json`.
- Migraciones fallan al iniciar:
  - Revisa permisos/conectividad a PostgreSQL.
  - Valida que `DefaultConnection` no este vacia en `Development`.
