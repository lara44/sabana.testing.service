# ARCHITECT.md — Luis Rojas
## Solution Architect

> Este documento describe quién soy, cómo pienso, qué construyo y cómo tomo decisiones arquitecturales.
> Sirve como contexto para la IA, onboarding de nuevos miembros del equipo, y referencia personal.

---

## Rol

Arquitecto de soluciones responsable de decisiones técnicas, diseño de sistemas y acompañamiento al equipo de desarrollo.

---

## Cómo tomo decisiones arquitecturales

### Orden de prioridad

1. ¿Resuelve el problema de negocio real?
2. ¿El equipo puede mantenerlo sin mí?
3. ¿Es explicable y defendible ante el cliente?
4. ¿Escala con el crecimiento esperado del sistema?
5. ¿La deuda técnica que introduce es aceptable y conocida?

### Proceso

- Identifico primero los atributos de calidad relevantes (disponibilidad, seguridad, mantenibilidad, observabilidad) antes de evaluar tecnologías.
- Valido la lógica con pseudocódigo o diagramas antes de traducir a código.
- Documento las decisiones como ADRs con contexto, alternativas consideradas y consecuencias.
- Antes de introducir un patrón nuevo, exijo que el equipo lo entienda. Un patrón que nadie más puede mantener no es una solución, es una deuda.
- No cambio un contrato o decisión acordada sin solicitarlo explícitamente.

---

## Principios que defiendo

### Arquitectura
- Clean Architecture es no negociable en proyectos nuevos: el dominio no depende de frameworks.
- CQRS y DDD se aplican cuando el dominio tiene complejidad real, no como dogma en CRUD simples.
- Los microservicios que no pueden desplegarse de forma independiente no son microservicios.
- Observabilidad desde el inicio, no al final. Sin logs estructurados y trazas, operar en producción es adivinar.
- La especificación (spec) precede al código. Lo que no está definido no se construye.

### Código
- Los handlers en Application no contienen detalles HTTP ni EF Core.
- Mappers manuales y explícitos — sin AutoMapper. La trazabilidad vale más que la comodidad.
- Constructor privado + factory `Create` para forzar invariantes desde la creación.
- `Rehydrate` para reconstrucción desde persistencia — semántica diferente a `Create`.
- El dominio nunca depende de Infrastructure, Presentation ni WebApi.

### Diagramas
- Todo sistema nuevo requiere al menos diagrama de Contexto y Contenedor (C4).
- Prefiero ver un diagrama antes que leer una explicación en prosa.
- PlantUML para diagramas versionables en el repositorio, draw.io para colaboración visual.
- Event Storming como punto de partida para modelado de dominio complejo.
- El nivel de detalle del diagrama debe corresponder al nivel de la decisión: no diagrames componentes si aún no está definido el contexto.
- Ver skill: `specs/architecture/skills/diagramming.md`

### Documentación
- Una arquitectura no documentada no existe — solo existe en la cabeza de quien la diseñó.
- Las decisiones arquitecturales se registran como ADRs con contexto, alternativas consideradas y consecuencias.
- Los procesos técnicos relevantes (migraciones, despliegues, configuración de entornos) deben estar documentados antes de que alguien más los necesite ejecutar.
- La documentación vive en el repositorio, no en chats ni correos.
- Un diagrama actualizado vale más que un documento extenso desactualizado.

### Equipo
- Un equipo que no entiende la arquitectura que implementa producirá bugs que nadie puede explicar.
- Las estimaciones deben ser defendibles y trazables — no números sacados del aire.
- Los one-on-ones son herramienta de liderazgo, no burocracia.

---

## Cómo evalúo una propuesta técnica

Cuando alguien (o una IA) me presenta una solución, pregunto:

- ¿Qué problema exactamente resuelve esto?
- ¿Qué pasa si falla?
- ¿Qué alternativas se consideraron y por qué se descartaron?
- ¿Qué pasa en 12 meses cuando el equipo cambia?
- ¿Cuánto tiempo toma entenderlo sin contexto previo?

Si no hay respuesta clara a estas preguntas, la propuesta no está lista.

---

## Instrucciones para la IA

- No modifiques nada que no haya sido explícitamente indicado. Si consideras que algo debe cambiar, señálalo y espera confirmación.
- Cuando propongas una solución, incluye siempre las alternativas que descartaste y por qué.
- Cuando se está definiendo una solución, propón primero un diagrama o pseudocódigo — no código completo.
- Para diagramas usa PlantUML si el resultado va al repositorio, draw.io si es para colaboración visual.
- Los ADRs son obligatorios para decisiones que afecten más de una capa o microservicio.
- No generes código sin que exista una spec o contrato definido previamente.
- Si una propuesta contradice una decisión ya documentada en este proyecto, señálalo explícitamente antes de proceder.
