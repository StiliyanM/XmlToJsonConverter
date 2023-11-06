using FluentValidation;
using System.Xml;
using XmlToJsonConverter.Web.Models;

namespace XmlToJsonConverter.Web.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            var errors = ex.Errors.Select(e => e.ErrorMessage);
            var errorMessage = string.Join(", ", errors);
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, errorMessage);
        }
        catch (XmlException ex)
        {
            _logger.LogError(ex, "Invalid XML format.");
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, 
                "Invalid XML format.");
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "File system error.");
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, 
                "Error processing file.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, 
                "An unexpected error occurred.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(
            new ErrorDetails(context.Response.StatusCode, message).ToString());
    }
}
