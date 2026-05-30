namespace sabana.testing.service.tests.unit.infrastructure.migrations;

using System.Reflection;
using Infrastructure.data.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

[TestClass]
public sealed class InitialCreateTests
{
    [TestMethod]
    public void Given_InitialMigration_When_UpIsInvoked_Then_RegistersProductsTableAndIndex()
    {
        // Arrange (Given)
        var migration = new InitialCreate();
        var migrationBuilder = new MigrationBuilder("Npgsql.EntityFrameworkCore.PostgreSQL");

        // Act (When)
        InvokeProtectedMethod(migration, "Up", migrationBuilder);

        // Assert (Then)
        Assert.HasCount(2, migrationBuilder.Operations);
        Assert.AreEqual("CreateTableOperation", migrationBuilder.Operations[0].GetType().Name);
        Assert.AreEqual("CreateIndexOperation", migrationBuilder.Operations[1].GetType().Name);
    }

    [TestMethod]
    public void Given_InitialMigration_When_DownIsInvoked_Then_RegistersDropTableOperation()
    {
        // Arrange (Given)
        var migration = new InitialCreate();
        var migrationBuilder = new MigrationBuilder("Npgsql.EntityFrameworkCore.PostgreSQL");

        // Act (When)
        InvokeProtectedMethod(migration, "Down", migrationBuilder);

        // Assert (Then)
        Assert.HasCount(1, migrationBuilder.Operations);
        Assert.AreEqual("DropTableOperation", migrationBuilder.Operations[0].GetType().Name);
    }

    [TestMethod]
    public void Given_InitialMigrationSnapshot_When_BuildTargetModelIsInvoked_Then_DefinesProductModelMetadata()
    {
        // Arrange (Given)
        var migration = new InitialCreate();
        var modelBuilder = new ModelBuilder(new ConventionSet());

        // Act (When)
        InvokeProtectedMethod(migration, "BuildTargetModel", modelBuilder);

        // Assert (Then)
        var entityType = modelBuilder.Model.FindEntityType("Infrastructure.data.Entities.ProductEntity");

        Assert.IsNotNull(entityType);
        Assert.AreEqual("Products", entityType.GetTableName());
        Assert.AreEqual(3, entityType.GetProperties().Count());
    }

    private static void InvokeProtectedMethod(object instance, string methodName, params object[] arguments)
    {
        var method = instance
            .GetType()
            .GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

        method!.Invoke(instance, arguments);
    }
}
