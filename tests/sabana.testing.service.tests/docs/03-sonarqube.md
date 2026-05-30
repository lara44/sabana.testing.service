# Guia de Analisis SonarQube

Esta guia explica solo el flujo de publicacion de analisis en SonarQube usando dotnet-sonarscanner.

## Prerrequisitos

- Tener instalado `dotnet-sonarscanner`.
- Tener token de acceso SonarQube.
- Tener configurado `SonarQube.Analysis.xml` en la raiz del repositorio.
- Tener disponible la guia de cobertura para generar `TestResults` y `Coverage`.

Instalacion del scanner (si aplica):

```bash
dotnet tool install --global dotnet-sonarscanner --version 11.1.0
```

## Flujo de analisis (dotnet-sonarscanner)

1. Iniciar contexto de analisis:

```bash
dotnet sonarscanner begin \
  /k:"ACP.Infocuenca.infocuenca-projectmanagementservice" \
  /s:"/ruta/absoluta/al/repositorio/SonarQube.Analysis.xml" \
  /d:sonar.login="YOUR_TOKEN"
```

2. Compilar solucion:

```bash
dotnet build
```

3. Ejecutar pruebas con cobertura:

```bash
dotnet test tests/sabana.testing.service.tests/sabana.testing.service.tests.csproj \
  --settings tests/sabana.testing.service.tests/test.runsettings \
  --results-directory TestResults \
  --collect:"XPlat Code Coverage"
```

4. Generar reportes de cobertura consumibles:

```bash
reportgenerator \
  -reports:"TestResults/*/coverage.opencover.xml" \
  -targetdir:"Coverage" \
  -reporttypes:"Html;Cobertura;SonarQube"
```

5. Finalizar y publicar en SonarQube:

```bash
dotnet sonarscanner end /d:sonar.login="YOUR_TOKEN"
```

## Notas importantes

- `begin` y `end` deben ejecutarse en el mismo contexto de trabajo.
- Si no se ejecuta `end`, el analisis no se publica.
- El path de `/s:` debe apuntar al archivo real `SonarQube.Analysis.xml` del repositorio.
- Si solo quieres cobertura local sin publicar, usa solo la guia de cobertura.
