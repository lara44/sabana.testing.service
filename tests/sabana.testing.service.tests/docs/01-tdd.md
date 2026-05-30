# sabana.testing.service.tests

Este proyecto contiene la estrategia de pruebas para `sabana.testing.service` con enfoque en Clean Architecture y TDD.

## Objetivo

- Separar pruebas por tipo (unit e integration).
- Hacer visible el proceso TDD: `rojo -> green -> refactor`.
- Mantener pruebas rapidas y confiables para reglas de negocio.

## Estructura

```text
tests/sabana.testing.service.tests/
  Unit/
    Domain/
    Application/
  Integration/
    Infrastructure/
    WebApi/
  docs/
    01-tdd.md
    02-coverage.md
    03-sonarqube.md
```

Regla general:
- `Unit`: sin base de datos real, sin red, sin host web.
- `Integration`: validan colaboracion real entre componentes (EF, DB, HTTP, DI, middleware).

## Stack de pruebas

- Framework: MSTest
- Comando principal: `dotnet test`
- Proyecto de pruebas: `tests/sabana.testing.service.tests/sabana.testing.service.tests.csproj`

## Flujo TDD documentado

### 1. Rojo (RED)

Se escribieron pruebas para reglas no implementadas del agregado `Product`:
- nombre vacio debe fallar
- precio no positivo debe fallar

Convenciones aplicadas desde esta fase:
- Patron AAA explicito en cada test: Arrange, Act, Assert.
- Nomenclatura Given-When-Then en el nombre de cada prueba.

Archivo de pruebas:
- `Unit/Domain/ProductAggregateTests.cs`

Comando de evidencia:

```bash
dotnet test --filter ProductAggregateTests
```

Resultado esperado en esta fase:
- pruebas en fallo (expected failures)

### 2. Green

Se implemento el minimo codigo en `Product.Create(...)` para pasar las pruebas:
- validacion de nombre obligatorio
- validacion de precio mayor que cero

Comando de validacion:

```bash
dotnet test --filter ProductAggregateTests
```

Resultado esperado:
- pruebas en verde

Pruebas implementadas actualmente para `Product.Create(...)`:
- `Given_EmptyName_When_Create_Then_ThrowsArgumentException`
- `Given_NonPositivePrice_When_Create_Then_ThrowsArgumentException`

### 3. Refactor

Se refactorizo la validacion sin cambiar comportamiento observable:
- extraccion de validaciones a metodos privados (`ValidateName`, `ValidatePrice`) en el agregado
- mantenimiento de AAA explicito en cada test (sin ocultar pasos en helpers compartidos)

Comando de regresion:

```bash
dotnet test --filter ProductAggregateTests
```

Resultado esperado:
- pruebas siguen en verde

## Convencion de commits recomendada

Para mostrar TDD de forma auditable, usar commits separados:

1. `test: red for product create validations`
2. `feat: green product create validations`
3. `refactor: extract product create validation methods`

## Ejecutar todas las pruebas

Desde la raiz del repositorio:

```bash
dotnet test
```

## Guias tecnicas

- Cobertura local: `docs/02-coverage.md`
- Analisis SonarQube: `docs/03-sonarqube.md`

## Notas

- Priorizar reglas de dominio en `Unit/Domain`.
- Casos de uso (handlers) en `Unit/Application` con dependencias simuladas.
- Repositorios EF y configuraciones en `Integration/Infrastructure`.
- Endpoints y middleware en `Integration/WebApi`.

## Criterios de evaluacion usados en este proyecto

- Cada ciclo TDD debe evidenciar: prueba en fallo, cambio minimo para pasar, y refactor sin cambio funcional.
- En pruebas unitarias de dominio se prioriza legibilidad explicita para rubrica academica sobre eliminar toda duplicacion.
- Given-When-Then define el nombre y AAA define la estructura interna del test.
