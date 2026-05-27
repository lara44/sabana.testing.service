using FluentValidation;
using Microsoft.AspNetCore.Http;
using Presentation.Common;

namespace WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Errores de validacion: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

            var validationErrors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            await HandleValidationExceptionAsync(context, validationErrors);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argumento invalido: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, $"Datos invalidos: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operacion invalida: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, $"Operacion invalida: {ex.Message}");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso no encontrado: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status404NotFound, $"Recurso no encontrado: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acceso no autorizado: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status403Forbidden, $"Acceso denegado: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Ha ocurrido un error inesperado en el servidor");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ResponseDto<object>
        {
            Code = statusCode,
            Error = true,
            Message = message,
            Data = null
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, Dictionary<string, string[]> validationErrors)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var response = new ResponseDto<Dictionary<string, string[]>>
        {
            Code = StatusCodes.Status400BadRequest,
            Error = true,
            Message = "Errores de validacion",
            Data = validationErrors
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
