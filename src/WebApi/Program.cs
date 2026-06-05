using Application;
using Microsoft.EntityFrameworkCore;
using Presentation;
using sabana.testing.service;
using WebApi;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();
builder.Services.AddScoped<IDatabaseInitializer, EfCoreDatabaseInitializer>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using var scope = app.Services.CreateScope();
var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();

await databaseInitializer.InitializeAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/", () => "Hello World!");
await app.RunAsync();

public partial class Program
{
    private Program()
    {
    }
}

