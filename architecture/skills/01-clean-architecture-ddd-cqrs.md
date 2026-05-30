# SKILL: clean-architecture-ddd-cqrs

## Purpose

Aplicar y mantener decisiones de arquitectura reutilizables con Clean Architecture + DDD + CQRS en .NET.

## When to use

- Crear/modificar funcionalidades de negocio.
- Diseñar nuevos casos de uso de lectura/escritura.
- Validar ubicacion correcta de codigo por capa.
- Revisar contratos, referencias entre proyectos y DI.

## Solution structure (reference)

```text
src/
  Domain/
    Abstractions/
      AggregateRoot.cs
      Entity.cs
    Aggregates/
      <AggregateName>/
        <AggregateRoot>.cs
        Repositories/
          I<AggregateName>Repository.cs
        ValueObjects/
  Application/
    DependencyInjection.cs
    <BoundedContext>/
      Commands/<UseCaseName>/
      Queries/<UseCaseName>/
  Infrastructure/
    DependencyInjection.cs
    data/
      ApplicationDbContext.cs
      Entities/
      Configuration/
      Migrations/
    Mappers/
    Repositories/
  Presentation/
    DependencyInjection.cs
    <BoundedContext>/
    Common/
  WebApi/
    Program.cs
    appsettings.json
    appsettings.Development.json
infrastructure/
  <environment-name>/
    docker-compose.yaml
```

## Project references and dependency direction

Direccion recomendada de dependencias (hacia adentro):

1. `Domain` -> no referencia otros proyectos internos.
2. `Application` -> referencia `Domain`.
3. `Infrastructure` -> referencia `Domain`.
4. `Presentation` -> referencia `Application`.
5. `WebApi` -> referencia `Infrastructure` y `Presentation` (composition root).

Regla:

- `Domain` nunca depende de `Infrastructure`, `Presentation` ni `WebApi`.

### Allowed vs forbidden references

- Allowed:
  - `Application -> Domain`
  - `Infrastructure -> Domain`
  - `Presentation -> Application`
  - `WebApi -> Infrastructure`
  - `WebApi -> Presentation`
- Forbidden:
  - `Domain -> *`
  - `Application -> Infrastructure|Presentation|WebApi`
  - `Infrastructure -> Application|Presentation|WebApi`
  - `Presentation -> Infrastructure|Domain|WebApi`

## Package policy by layer

- `Domain`:
  - solo BCL/net runtime.
  - prohibido EF Core, ASP.NET Core, librerias de transporte/mensajeria.
- `Application`:
  - mediator y contratos de aplicacion.
  - sin EF Core ni dependencias de HTTP.
- `Infrastructure`:
  - EF Core provider-specific, mappers, acceso a datos.
- `Presentation`:
  - ASP.NET Core MVC/controller surface.
- `WebApi`:
  - composition root, OpenAPI/Swagger, middleware y configuracion.

## Domain conventions (DDD)

### 1) Carpetas base

- `Abstractions/`: clases base reutilizables de dominio.
- `Aggregates/<AggregateName>/`: modelo de cada agregado.
- Dentro de cada agregado:
  - raiz de agregado,
  - `Repositories/` para contratos,
  - `ValueObjects/` cuando aplique.

### 2) Entity and AggregateRoot

- `Entity<TEntityId>` define `Id` tipado.
- `AggregateRoot<TEntityId>` hereda de `Entity<TEntityId>`.
- `TEntityId` por defecto en este proyecto: `Guid`.

Regla recomendada:

- usar un solo tipo de id por agregado (ej. `Guid`) para evitar conversiones y ambiguedad entre capas.

### 3) Constructor privado + factory static `Create`

Motivo:

- Forzar invariantes al crear instancias nuevas.
- Evitar objetos en estado invalido desde afuera.

Patron aplicado:

- `private <AggregateRoot>(Guid id, ...)` para construccion controlada.
- `public static <AggregateRoot> Create(...)` para alta de entidad nueva.
- `Create` valida reglas y genera `Guid.NewGuid()`.

### 4) Rehydrate (reconstruccion)

- `public static <AggregateRoot> Rehydrate(id, ...)` para reconstruir desde persistencia sin semantica de "nuevo".
- Usar en mappers/repositorios, no en flujos de negocio de creacion.

### 5) Estructura interna por agregado

Convencion sugerida por agregado:

```text
Domain/Aggregates/<AggregateName>/
  <AggregateRoot>.cs
  Repositories/
    I<AggregateName>Repository.cs
  ValueObjects/
  DomainEvents/
```

Notas:

- `ValueObjects/` y `DomainEvents/` son opcionales y se crean cuando el negocio lo requiera.

## Application conventions (CQRS)

### 1) Estructura de casos de uso

Por feature:

```text
Application/<BoundedContext>/
  Commands/<CreateSomething>/
    <CreateSomething>Command.cs
    <CreateSomething>CommandHandler.cs
    <CreateSomething>Result.cs
  Queries/<GetSomething>/
    <GetSomething>Query.cs
    <GetSomething>QueryHandler.cs
    <GetSomething>Result.cs
```

### 2) Mediator

- Opcion recomendada (actual en este repo): `AtcMediator`.
- Registro en `Application/DependencyInjection.cs` con `AddATCMediator(Assembly.GetExecutingAssembly())`.
- Alternativa valida: `MediatR`, manteniendo la misma separacion Command/Query.

### 3) Regla de handlers

- Handlers delgados:
  - reciben request,
  - orquestan repositorios/puertos,
  - mapean respuesta,
  - sin detalles HTTP ni EF Core.

### 4) Contratos de entrada/salida

- `Command`/`Query` en Application deben representar caso de uso, no transporte HTTP.
- `Result` en Application debe exponer solo datos necesarios para la capa de presentacion.
- Validaciones de invariantes de negocio viven en Domain, no en handlers.

## Infrastructure conventions

### 1) Subcarpetas obligatorias

- `data/Entities/`: entidades de persistencia.
- `data/Configuration/`: `IEntityTypeConfiguration<T>`.
- `data/Migrations/`: migraciones EF Core.
- `data/ApplicationDbContext.cs`: DbContext principal.
- `Repositories/`: implementaciones concretas de contratos de dominio.
- `Mappers/`: mapeo manual entre dominio y datos.

### 2) ApplicationDbContext

- Expone `DbSet<...>` de entidades.
- `OnModelCreating` aplica configuraciones del assembly con `ApplyConfigurationsFromAssembly`.

Regla:

- las entidades de `data/Entities` son de persistencia y no deben filtrarse a Domain/Application.
- el tipo de llave primaria en persistencia debe ser consistente con el tipo de `Id` del agregado (ej. `Guid` en dominio -> `Guid` en entidad de datos).

### 3) Mapeo manual

- Decisión: mapeo manual en `Infrastructure/Mappers/*Mapper.cs`.
- No usar AutoMapper u otras librerias para este contexto.
- Ventaja buscada: trazabilidad y control explicito del mapeo.

### 4) EF Core libraries

Paquetes base en `Infrastructure.csproj`:

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`

Notas operativas:

- migraciones viven en `data/Migrations/`.
- configuraciones por entidad en `data/Configuration/` con `IEntityTypeConfiguration<T>`.
- definir tipo de columna segun proveedor en configuracion (ej. PostgreSQL: `uuid` para id Guid, `numeric(p,s)` para montos).

### 5) Migration policy and workflow

Reglas:

- todas las migraciones de EF Core se generan y almacenan en `Infrastructure/data/Migrations/`.
- la capa dueña de migraciones es `Infrastructure` (nunca `Domain` ni `Application`).
- una migracion debe corresponder a un cambio intencional del modelo de persistencia.
- si cambia el modelo de datos, actualizar migracion y validar que `ApplicationDbContextModelSnapshot` quede consistente.

Comandos recomendados (desde la raiz):

```bash
dotnet ef migrations add <MigrationName> \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/WebApi/WebApi.csproj \
  --output-dir data/Migrations
```

Aplicar migraciones manualmente:

```bash
dotnet ef database update \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/WebApi/WebApi.csproj
```

Revertir a una migracion anterior:

```bash
dotnet ef database update <PreviousMigrationName> \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/WebApi/WebApi.csproj
```

Eliminar ultima migracion (si aun no fue aplicada en entornos compartidos):

```bash
dotnet ef migrations remove \
  --project src/Infrastructure/Infrastructure.csproj \
  --startup-project src/WebApi/WebApi.csproj
```

Notas de uso con startup migration:

- `WebApi/Program.cs` puede ejecutar `db.Database.MigrateAsync()` para entorno local/dev.
- en produccion, preferir pipeline controlado de migraciones para reducir riesgo operativo.
- si se usa `MigrateAsync` en runtime, monitorear timeout, locks y orden de despliegue.

### 6) Repository implementation rules

- Los repositorios implementan interfaces definidas en `Domain`.
- Deben mapear `Domain <-> Data` mediante mappers manuales.
- No mover reglas de negocio a repositorio.

### 7) DependencyInjection de Infrastructure

En `Infrastructure/DependencyInjection.cs`:

- Registrar `ApplicationDbContext` con `UseNpgsql`.
- Leer `DefaultConnection` de configuracion.
- Registrar repositorios concretos (`AddScoped<I<AggregateName>Repository, <AggregateName>Repository>`).

## Presentation and WebApi conventions

### Presentation

- `Presentation/DependencyInjection.cs` registra controllers por `AddApplicationPart`.
- Controladores delgados: delegan al mediador.

Regla:

- Presentation maneja contratos HTTP (status code, DTO de respuesta, route), no reglas de negocio.

### WebApi (composition root)

- `Program.cs` centraliza:
  - `AddInfrastructure(builder.Configuration)`
  - `AddApplication()`
  - `AddPresentation()`
- Ejecuta migraciones al inicio con `db.Database.MigrateAsync()`.

Cross-cutting recomendado:

- manejar excepciones en middleware global (`ExceptionHandlingMiddleware`) y mapear a respuestas consistentes.

## Runtime environment and local infra

### 0) .NET and C# version policy

Regla recomendada para estabilidad de equipo:

- fijar SDK con `global.json` en una version concreta.
- evitar `rollForward` amplio para no introducir cambios inesperados.

Ejemplo estable estricto:

```json
{
  "sdk": {
    "version": "10.0.101",
    "rollForward": "disable"
  }
}
```

Ejemplo equilibrado (permite parches de seguridad):

```json
{
  "sdk": {
    "version": "10.0.101",
    "rollForward": "latestPatch"
  }
}
```

Convencion de C#:

- evitar `LangVersion=latest` en proyectos colaborativos.
- preferir la version por defecto del `TargetFramework` (o fijar una version explicita solo si hay necesidad real).

### 1) Docker DB

- Definir en `infrastructure/<environment-name>/docker-compose.yaml`.
- Sugerencia base para PostgreSQL local:
  - port host `<db-host-port>` -> container `5432`
  - user/password `<db-user>/<db-password>`
  - database `<db-name>`

### 2) appsettings.Development

- `src/WebApi/appsettings.Development.json` debe definir:
  - `ConnectionStrings:DefaultConnection`
- Ejemplo de cadena local:
  - `Host=localhost;Port=<db-host-port>;Database=<db-name>;Username=<db-user>;Password=<db-password>`

## Git ignore recommendations

Objetivo:

- evitar versionar artefactos generados (build, test, coverage, outputs locales).

Patrones recomendados minimos en `.gitignore`:

```gitignore
**/[Bb]in/
**/[Oo]bj/
*.dll
*.pdb
*.trx
/TestResults/
/Coverage/
/StrykerOutput/
.vs/
```

Si ya hay archivos rastreados, dejarlos de rastrear sin borrarlos del disco:

```bash
git rm -r --cached TestResults Coverage StrykerOutput
git commit -m "chore: stop tracking generated artifacts"
```

Nota:

- el `.gitignore` evita nuevos archivos, pero no elimina del indice lo que ya estaba versionado.

## Implementation playbook (new feature)

1. Modelar dominio:
  - crear/agregar agregado en `Domain/Aggregates/<AggregateName>/`.
  - definir invariantes en `Create` y contratos de repositorio en `Repositories/`.
2. Crear casos de uso:
  - agregar `Command/Query` + `Handler` + `Result` en `Application/<BoundedContext>/...`.
3. Implementar persistencia:
  - crear entidad de datos + configuracion EF + repo concreto + mapper manual en `Infrastructure`.
4. Registrar DI:
  - actualizar `Infrastructure/DependencyInjection.cs` y `Application/DependencyInjection.cs` si aplica.
5. Exponer endpoint:
  - crear/ajustar controller en `Presentation`.
6. Componer y ejecutar:
  - verificar `WebApi/Program.cs`, configuracion de `DefaultConnection` y docker local.

## Architecture definition of done

- Regla de negocio ubicada en capa correcta.
- Referencias entre proyectos respetan direccion de dependencias.
- Dominio mantiene invariantes y constructor controlado.
- Casos de uso separados en Command/Query.
- Infrastructure contiene `data`, `mappers` y `repositories` explicitos.
- DI por capa centralizada en `Program.cs`.
- Configuracion local documentada (docker + appsettings).
- Pruebas unitarias cubren cambios relevantes.

## Review checklist

- ¿`Domain` quedo libre de dependencias a frameworks?
- ¿El agregado usa `Create` para nuevas instancias y `Rehydrate` para lectura?
- ¿Hay contrato de repositorio en `Domain` e implementacion en `Infrastructure`?
- ¿Los mappers son manuales y explicitos?
- ¿Los handlers en `Application` no contienen detalles HTTP/EF?
- ¿`Infrastructure/DependencyInjection.cs` registra DbContext y repositorios?
- ¿`WebApi/Program.cs` compone capas y aplica migraciones?
- ¿Existe configuracion local valida en `appsettings.Development.json` y docker-compose?
