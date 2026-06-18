# Pruebas de carga con k6

Este directorio contiene el script de carga:
- `sabana-testing-load.js`

## Instalacion de k6 (macOS)

Requiere Homebrew.

```bash
brew install k6
k6 version
```

## Levantar la API antes de ejecutar k6

Desde la raiz del proyecto:

```bash
dotnet run --project src/WebApi
```

## Ejecutar el script

### Opcion 1: desde la raiz del proyecto

```bash
k6 run tests/sabana.testing.service.tests.k6/sabana-testing-load.js
```

### Opcion 2: desde este directorio

```bash
cd tests/sabana.testing.service.tests.k6
k6 run sabana-testing-load.js
```

## Ejecutar con variables de entorno

```bash
k6 run -e HOST=localhost -e PORT=5121 sabana-testing-load.js
```

## Exportar resultados a CSV

```bash
mkdir -p results
k6 run --out csv=results/k6-results.csv sabana-testing-load.js
```

## Duracion actual de la prueba

Los 2 escenarios corren en paralelo y cada uno dura ~60 segundos:
- Escenario 1: 15s ramp-up + 40s carga + 5s ramp-down
- Escenario 2: 10s ramp-up + 45s carga + 5s ramp-down
