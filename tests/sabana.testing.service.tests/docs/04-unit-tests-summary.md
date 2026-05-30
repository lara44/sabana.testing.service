# Resumen de Pruebas Unitarias

Este documento presenta un resumen de todas las pruebas unitarias implementadas en el proyecto de pruebas.

## Objetivo

- Tener una vista rapida de la cobertura funcional por caso de prueba.
- Identificar que componente se verifica y que regla valida cada test.
- Mantener trazabilidad entre TDD, codigo y cobertura.

## Resumen por prueba

<table style="width: 100%; table-layout: fixed; border-collapse: collapse;">
	<colgroup>
		<col style="width: 42%;" />
		<col style="width: 14%;" />
		<col style="width: 18%;" />
		<col style="width: 26%;" />
	</colgroup>
	<thead>
		<tr>
			<th style="background: #000; color: #fff; padding: 8px; border: 1px solid #000;">Prueba unitaria</th>
			<th style="background: #000; color: #fff; padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Componente verificado</th>
			<th style="background: #000; color: #fff; padding: 8px; border: 1px solid #000;">Regla de negocio o regla tecnica</th>
			<th style="background: #000; color: #fff; padding: 8px; border: 1px solid #000;">Propósito</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_EmptyName_When_Create_Then_ThrowsArgumentException</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Product.Create()</td>
			<td style="padding: 8px; border: 1px solid #000;">Nombre obligatorio</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que no se permite crear un producto sin nombre</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_NonPositivePrice_When_Create_Then_ThrowsArgumentException</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Product.Create()</td>
			<td style="padding: 8px; border: 1px solid #000;">Precio mayor que cero</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que no se permite crear un producto con precio invalido</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_ValidCreateProductCommand_When_HandleAsync_Then_ReturnsMappedCreateProductResult</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">CreateProductCommandHandler</td>
			<td style="padding: 8px; border: 1px solid #000;">Flujo feliz de creacion exitosa</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el caso de uso crea el producto, llama al repositorio y mapea la respuesta</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_ProductsStoredInRepository_When_HandleAsync_Then_ReturnsMappedProducts</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">GetProductsQueryHandler</td>
			<td style="padding: 8px; border: 1px solid #000;">Flujo feliz de consulta de productos</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el caso de uso consulta el repositorio y mapea la respuesta</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_ServiceCollection_When_AddApplication_Then_ReturnsSameServiceCollection</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Application.DependencyInjection.AddApplication()</td>
			<td style="padding: 8px; border: 1px solid #000;">Registro de dependencias</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el metodo de extension preserva la coleccion de servicios</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_ServiceCollection_When_AddInfrastructure_Then_ReturnsSameServiceCollection</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Infrastructure.DependencyInjection.AddInfrastructure()</td>
			<td style="padding: 8px; border: 1px solid #000;">Registro de dependencias</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el metodo de extension preserva la coleccion de servicios</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_ServiceCollection_When_AddPresentation_Then_ReturnsSameServiceCollection</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Presentation.DependencyInjection.AddPresentation()</td>
			<td style="padding: 8px; border: 1px solid #000;">Registro de dependencias</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el metodo de extension preserva la coleccion de servicios</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_NoArguments_When_CreateDefaultResponse_Then_UsesDefaultValues</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">ResponseDto&lt;T&gt;</td>
			<td style="padding: 8px; border: 1px solid #000;">Inicializacion por defecto</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma los valores base del DTO</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_InputValues_When_UseFactoryMethods_Then_ReturnExpectedResponses</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">ResponseDto&lt;T&gt;</td>
			<td style="padding: 8px; border: 1px solid #000;">Fabricas de respuesta</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que los metodos de fabrica construyen respuestas correctas para exito y errores</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_DomainProduct_When_ToDataEntity_Then_ReturnsMappedEntity</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">ProductMapper.ToDataEntity()</td>
			<td style="padding: 8px; border: 1px solid #000;">Mapeo dominio -&gt; datos</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el mapper traduce correctamente el agregado a entidad de persistencia</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_DataProductEntity_When_ToDomainEntity_Then_ReturnsMappedProduct</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">ProductMapper.ToDomainEntity()</td>
			<td style="padding: 8px; border: 1px solid #000;">Mapeo datos -&gt; dominio</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el mapper reconstruye correctamente el agregado de dominio</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_NewProduct_When_AddAsync_Then_PersistsAndReturnsMappedProduct</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">ProductRepository.AddAsync()</td>
			<td style="padding: 8px; border: 1px solid #000;">Persistencia de producto</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el repositorio guarda la entidad y devuelve el agregado mapeado</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_PersistedProducts_When_GetAllAsync_Then_ReturnsMappedProducts</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">ProductRepository.GetAllAsync()</td>
			<td style="padding: 8px; border: 1px solid #000;">Consulta de productos</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el repositorio devuelve la coleccion mapeada desde la base de datos in-memory</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_InitialMigration_When_UpIsInvoked_Then_RegistersProductsTableAndIndex</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">InitialCreate.Up()</td>
			<td style="padding: 8px; border: 1px solid #000;">Definicion de esquema</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que la migracion crea la tabla e indice esperados</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_InitialMigration_When_DownIsInvoked_Then_RegistersDropTableOperation</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">InitialCreate.Down()</td>
			<td style="padding: 8px; border: 1px solid #000;">Reversion de esquema</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que la migracion elimina la tabla esperada</td>
		</tr>
		<tr>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">Given_InitialMigrationSnapshot_When_BuildTargetModelIsInvoked_Then_DefinesProductModelMetadata</td>
			<td style="padding: 8px; border: 1px solid #000; word-break: break-word; overflow-wrap: anywhere;">InitialCreate.BuildTargetModel()</td>
			<td style="padding: 8px; border: 1px solid #000;">Modelo EF snapshot</td>
			<td style="padding: 8px; border: 1px solid #000;">Confirma que el snapshot del modelo define la entidad <code>ProductEntity</code> correctamente</td>
		</tr>
	</tbody>
</table>

## Resumen por area

| Area | Archivos principales | Cantidad de pruebas |
| --- | --- | --- |
| `Domain` | `Unit/Domain/ProductAggregateTests.cs` | 2 |
| `Application` | `Unit/Application/DependencyInjectionTests.cs`, `Unit/Application/Products/...` | 3 |
| `Presentation` | `Unit/Presentation/DependencyInjectionTests.cs`, `Unit/Presentation/Common/ResponseDtoTests.cs` | 3 |
| `Infrastructure` | `Unit/Infrastructure/DependencyInjectionTests.cs`, `Unit/Infrastructure/Mappers/...`, `Unit/Infrastructure/Repositories/...`, `Unit/Infrastructure/Migrations/...` | 8 |

## Relacion con TDD

- Las pruebas de `Domain` reflejan el ciclo RED -> GREEN -> REFACTOR del agregado `Product`.
- Las pruebas de `Application` validan el comportamiento de los casos de uso de productos.
- Las pruebas de `Presentation` e `Infrastructure` completan el panorama unitario con reglas tecnicas y de adaptacion.

## Documentacion relacionada

- TDD y estructura del proyecto: `docs/01-tdd.md`
- Cobertura local: `docs/02-coverage.md`
- Analisis SonarQube: `docs/03-sonarqube.md`
