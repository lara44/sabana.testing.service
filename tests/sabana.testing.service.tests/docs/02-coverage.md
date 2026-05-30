# Guia de Cobertura Local

Esta guia cubre solo la ejecucion de pruebas con cobertura y la generacion de reportes locales.

## Archivos y carpetas involucradas

- `tests/sabana.testing.service.tests/test.runsettings`:
  - define formato de cobertura OpenCover,
  - configura exclusiones por atributos,
  - habilita loggers `console`, `trx` y `junit`.
- `TestResults/`:
  - se genera al ejecutar `dotnet test` con `--results-directory TestResults`,
  - contiene resultados de pruebas (`test-results.trx.xml`, `test-results.junit.xml`),
  - contiene subcarpetas con GUID y el archivo de cobertura raw (`coverage.opencover.xml`).
- `Coverage/`:
  - se genera con ReportGenerator,
  - contiene reporte navegable `index.html`,
  - contiene formatos de salida para otras herramientas (`Cobertura.xml`, `SonarQube.xml`).

## Flujo local recomendado

1. Ejecutar pruebas con cobertura:

```bash
dotnet test tests/sabana.testing.service.tests/sabana.testing.service.tests.csproj \
  --settings tests/sabana.testing.service.tests/test.runsettings \
  --results-directory TestResults \
  --collect:"XPlat Code Coverage"
```

2. Generar reportes de cobertura:

```bash
reportgenerator \
  -reports:"TestResults/*/coverage.opencover.xml" \
  -targetdir:"Coverage" \
  -reporttypes:"Html;Cobertura;SonarQube"
```

3. Abrir reporte en macOS:

```bash
open Coverage/index.html
```

## Herramientas necesarias

Si no tienes ReportGenerator instalado:

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

## Alcance de esta guia

- Esta guia no publica resultados a SonarQube.
- Para el flujo de scanner (`begin -> build -> test -> end`), ver la guia separada en `docs/03-sonarqube.md`.
