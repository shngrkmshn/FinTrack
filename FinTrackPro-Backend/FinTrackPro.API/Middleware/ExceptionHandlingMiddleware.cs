using FinTrackPro.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackPro.API.Middleware;

public sealed class ExceptionHandlingMiddleware
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
        catch (Exception exception)
        {
            LogException(exception);

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = exception switch
        {
            ValidationException validationException => BuildValidationProblemDetails(validationException),

            NotFoundException notFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = notFoundException.Message,
            },

            UnauthorizedException unauthorizedException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = unauthorizedException.Message,
            },

            DomainException domainException => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Domain Rule Violation",
                Detail = domainException.Message,
            },

            ArgumentException argumentException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = argumentException.Message,
            },

            InvalidOperationException invalidOperationException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = invalidOperationException.Message,
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred.",
            }
        };

        context.Response.StatusCode = problemDetails.Status!.Value;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private void LogException(Exception exception)
    {
        switch (exception)
        {
            case ValidationException:
            case UnauthorizedException:
                _logger.LogWarning(exception, "{Message}", exception.Message);
                break;
            default:
                _logger.LogError(exception, "{Message}", exception.Message);
                break;
        }
    }

    private static ValidationProblemDetails BuildValidationProblemDetails(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
        };
    }
}
