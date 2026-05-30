# SKILL: testing-tdd-aaa-coverage

## Purpose

Definir una estrategia de pruebas reutilizable con TDD, patron AAA, convencion Given-When-Then, cobertura y preparacion para SonarQube.

## When to use

- Crear o modificar pruebas unitarias/integracion.
- Diseñar flujo de calidad para nuevas features.
- Subir cobertura con criterios explicitos.
- Estandarizar evidencias para evaluacion tecnica o academica.

## Testing architecture (reference)

```text
tests/<test-project>/
  Unit/
    Domain/
    Application/
    Infrastructure/
    Presentation/
  Integration/
    Infrastructure/
    WebApi/
  docs/
  test.runsettings
```

## Testing conventions

### 1) Naming

- formato obligatorio: `Given_X_When_Y_Then_Z`.
- el nombre debe describir comportamiento observable, no detalle interno.

### 2) AAA structure

- cada test debe mostrar explicitamente:
  - `Arrange`
  - `Act`
  - `Assert`
- en pruebas pequenas puede combinarse `Act + Assert` solo si la legibilidad mejora.

### 3) Readability vs DRY

- preferir legibilidad y trazabilidad sobre eliminar toda duplicacion.
- extraer helpers solo cuando reduzcan complejidad real, no para ocultar el flujo AAA.

### 4) Determinism

- evitar dependencias de reloj, red y filesystem real en pruebas unitarias.
- evitar estado compartido entre tests.
- usar ids constantes o semilla controlada cuando importe orden/comparacion.

## TDD workflow policy

1. RED:
  - escribir una prueba que falle por la razon correcta.
  - el fallo debe representar una regla faltante o mal implementada.
2. GREEN:
  - implementar el minimo codigo para pasar.
  - evitar mejoras no requeridas en esta fase.
3. REFACTOR:
  - mejorar diseno interno sin cambiar comportamiento.
  - mantener suite en verde en todo momento.

Regla:

- no mezclar feature nueva y refactor grande en el mismo ciclo.

## Test taxonomy by layer

### Domain (unit)

- validar invariantes y comportamiento del agregado/entidad.
- probar casos invalidos + caso feliz.
- no usar EF/HTTP/DI container.

### Application (unit)

- probar handlers de command/query con doubles/fakes de puertos.
- verificar:
  - llamada a dependencia esperada,
  - propagacion de `CancellationToken`,
  - mapeo correcto a `Result`.

### Infrastructure (unit tecnico)

- probar mappers manuales.
- probar repositorios con provider in-memory cuando aplique.
- probar configuraciones/migraciones con aserciones sobre metadatos/operations.

### Presentation (unit)

- probar DTOs/factories de respuesta.
- probar extensiones de DI.
- evitar host web real en unit tests.

### Integration

- usar cuando se valida colaboracion real (DB, middleware, host, serialization).
- mantener separadas de unit para evitar feedback lento.

## Doubles strategy

- preferencia inicial: fakes manuales pequeños y explicitos.
- usar mocks/framework solo cuando el comportamiento sea complejo.
- no verificar interacciones irrelevantes.

## Coverage policy

### Goal interpretation

- `line coverage`: KPI principal de avance.
- `branch coverage`: complemento para evaluar caminos logicos.
- objetivo recomendado: subir ambos, priorizando line coverage.

### What to include

- incluir codigo de negocio y adaptadores relevantes.
- evitar inflar cobertura con pruebas artificiales sin valor.

### What to exclude (segun contexto)

- artefactos generados (ej. migrations/snapshots) solo si existe criterio formal de exclusion.
- si se incluyen, documentar por que y como se validan.

## Runsettings policy

`test.runsettings` debe centralizar:

- data collector `XPlat Code Coverage`.
- formato de salida de cobertura (`opencover` recomendado).
- loggers (`console`, `trx`, `junit` si aplica).
- exclusiones por atributos generados.

## Coverage workflow (local)

1. Ejecutar pruebas con cobertura:

```bash
dotnet test tests/<test-project>/<test-project>.csproj \
  --settings tests/<test-project>/test.runsettings \
  --results-directory TestResults \
  --collect:"XPlat Code Coverage"
```

2. Generar reportes:

```bash
reportgenerator \
  -reports:"TestResults/*/coverage.opencover.xml" \
  -targetdir:"Coverage" \
  -reporttypes:"Html;Cobertura;SonarQube"
```

3. Abrir reporte local (macOS):

```bash
open Coverage/index.html
```

## SonarQube readiness

Requisitos minimos:

- archivo de analisis (`SonarQube.Analysis.xml` o equivalente).
- TRX disponible en `TestResults/`.
- OpenCover disponible en `TestResults/*/coverage.opencover.xml`.

Flujo tipico de scanner:

1. `dotnet sonarscanner begin ...`
2. `dotnet build`
3. `dotnet test ... --collect:"XPlat Code Coverage"`
4. `reportgenerator ... -reporttypes:"...;SonarQube"`
5. `dotnet sonarscanner end ...`

## Git hygiene for testing artifacts

Minimo recomendado en `.gitignore`:

```gitignore
/TestResults/
/Coverage/
/StrykerOutput/
*.trx
coverage*.xml
```

Si ya estaban versionados:

```bash
git rm -r --cached TestResults Coverage StrykerOutput
```

## Implementation playbook (new testable feature)

1. definir criterio de aceptacion.
2. escribir test RED en capa correcta.
3. implementar GREEN minimo.
4. refactorizar manteniendo verde.
5. agregar pruebas colindantes (errores, borde, caso feliz).
6. ejecutar suite + cobertura.
7. actualizar docs de pruebas si cambian reglas/flujo.

## Definition of done (testing)

- pruebas nuevas verdes localmente.
- nombres Given-When-Then coherentes.
- AAA visible y legible.
- cobertura regenerada y revisada.
- sin artefactos generados trackeados en git.
- documentacion tecnica de pruebas actualizada.

## Review checklist

- ¿la prueba falla por razon correcta en RED?
- ¿GREEN agrego solo codigo minimo?
- ¿REFACTOR no cambio comportamiento?
- ¿la capa de prueba corresponde a la responsabilidad del cambio?
- ¿se cubren caso feliz + invalidos relevantes?
- ¿se propaga cancellation token cuando aplica?
- ¿coverage report fue regenerado y revisado?
- ¿artefactos de test/cobertura quedaron fuera de git?
